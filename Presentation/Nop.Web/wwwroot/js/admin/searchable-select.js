var SearchableSelect = (function () {
  function qs(root, selector) {
    return root ? root.querySelector(selector) : null;
  }

  function qsa(root, selector) {
    return root ? Array.prototype.slice.call(root.querySelectorAll(selector)) : [];
  }

  function show(element) {
    if (element) {
      element.hidden = false;
    }
  }

  function hide(element) {
    if (element) {
      element.hidden = true;
    }
  }

  function getAntiForgeryToken() {
    var tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    return tokenInput ? tokenInput.value : '';
  }

  function postJson(url, body) {
    return fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-Requested-With': 'XMLHttpRequest',
        'RequestVerificationToken': getAntiForgeryToken()
      },
      body: JSON.stringify(body)
    }).then(function (response) {
      if (!response.ok) {
        throw new Error('HTTP ' + response.status);
      }

      return response.json();
    });
  }

  function createIcon(className) {
    var icon = document.createElement('i');
    icon.className = className;
    icon.setAttribute('aria-hidden', 'true');
    return icon;
  }

  function bind(wrapper) {
    if (!wrapper || wrapper.getAttribute('data-ss-bound')) {
      return;
    }

    wrapper.setAttribute('data-ss-bound', '1');

    var saveUrl = wrapper.getAttribute('data-save-url');
    var deleteUrl = wrapper.getAttribute('data-delete-url');
    var category = wrapper.getAttribute('data-category');
    var placeholder = wrapper.getAttribute('data-placeholder') || 'Select';
    var hiddenInput = qs(wrapper, '.ss-hidden-input');
    var trigger = qs(wrapper, '.ss-trigger');
    var triggerText = qs(wrapper, '.ss-trigger-text');
    var panel = qs(wrapper, '.ss-panel');
    var searchInput = qs(wrapper, '.ss-search');
    var list = qs(wrapper, '.ss-list');
    var emptyMessage = qs(wrapper, '.ss-empty');
    var addTrigger = qs(wrapper, '.ss-add-trigger');
    var addForm = qs(wrapper, '.ss-add-form');
    var addInput = qs(wrapper, '.ss-add-input');
    var addSaveButton = qs(wrapper, '.ss-add-save');
    var addCancelButton = qs(wrapper, '.ss-add-cancel');
    var savingIndicator = qs(wrapper, '.ss-saving-indicator');

    if (!hiddenInput || !trigger || !triggerText || !panel || !searchInput || !list) {
      return;
    }

    function removeError() {
      var error = qs(wrapper, '.ss-error-msg');
      if (error) {
        error.parentNode.removeChild(error);
      }
    }

    function showError(message) {
      var footer;
      var error;

      removeError();
      footer = qs(wrapper, '.ss-footer');
      if (!footer) {
        return;
      }

      error = document.createElement('span');
      error.className = 'ss-error-msg';
      error.textContent = message;
      footer.appendChild(error);
    }

    function filterOptions(term) {
      var query = (term || '').toLowerCase().replace(/^\s+|\s+$/g, '');
      var visible = 0;

      qsa(list, '.ss-option').forEach(function (option) {
        var textNode = qs(option, '.ss-option-text');
        var text = textNode ? textNode.textContent.toLowerCase() : '';
        var match = !query || text.indexOf(query) !== -1;

        if (match) {
          option.classList.remove('ss-hidden');
          visible += 1;
        } else {
          option.classList.add('ss-hidden');
        }
      });

      if (emptyMessage) {
        emptyMessage.hidden = visible > 0;
      }
    }

    function resetAddForm() {
      hide(addForm);
      hide(savingIndicator);
      show(addTrigger);
      if (addInput) {
        addInput.value = '';
      }
      removeError();
    }

    function closePanel() {
      wrapper.classList.remove('ss-open');
      trigger.setAttribute('aria-expanded', 'false');
      hide(panel);
      resetAddForm();
    }

    function closeOtherPanels() {
      qsa(document, '.ss-wrapper.ss-open').forEach(function (openWrapper) {
        var openTrigger;
        var openPanel;

        if (openWrapper === wrapper) {
          return;
        }

        openTrigger = qs(openWrapper, '.ss-trigger');
        openPanel = qs(openWrapper, '.ss-panel');

        if (openTrigger) {
          openTrigger.setAttribute('aria-expanded', 'false');
        }

        if (openPanel) {
          openPanel.hidden = true;
        }

        openWrapper.classList.remove('ss-open');
      });
    }

    function openPanel() {
      closeOtherPanels();
      wrapper.classList.add('ss-open');
      trigger.setAttribute('aria-expanded', 'true');
      show(panel);
      searchInput.value = '';
      filterOptions('');
      searchInput.focus();
    }

    function togglePanel() {
      if (wrapper.classList.contains('ss-open')) {
        closePanel();
      } else {
        openPanel();
      }
    }

    function selectOption(option) {
      var textNode;

      qsa(list, '.ss-option.ss-selected').forEach(function (selectedOption) {
        selectedOption.classList.remove('ss-selected');
        selectedOption.setAttribute('aria-selected', 'false');
      });

      option.classList.add('ss-selected');
      option.setAttribute('aria-selected', 'true');

      textNode = qs(option, '.ss-option-text');
      hiddenInput.value = option.getAttribute('data-value') || '';
      triggerText.textContent = textNode ? textNode.textContent : placeholder;
      triggerText.classList.remove('ss-placeholder');

      hiddenInput.dispatchEvent(new Event('change', { bubbles: true }));
      closePanel();
    }

    function buildUndoButton(optionId, option) {
      var button = document.createElement('button');
      button.type = 'button';
      button.className = 'ss-undo-btn';
      button.title = 'Undo';
      button.appendChild(createIcon('fas fa-rotate-left'));

      button.addEventListener('click', function (event) {
        event.stopPropagation();
        button.disabled = true;

        postJson(deleteUrl, { id: Number(optionId), category: category }).then(function (data) {
          if (!data.success) {
            throw new Error(data.message || 'Could not undo');
          }

          document.dispatchEvent(new CustomEvent('searchableSelect:optionRemoved', {
            detail: {
              category: category,
              value: option.getAttribute('data-value')
            }
          }));
        }).catch(function (error) {
          button.disabled = false;
          showError(error.message);
        });
      });

      return button;
    }

    function buildOption(value, text, optionId, createdInSession) {
      var option = document.createElement('li');
      var textSpan = document.createElement('span');

      option.className = 'ss-option';
      option.setAttribute('data-value', String(value));
      option.setAttribute('data-option-id', String(optionId || ''));
      option.setAttribute('role', 'option');
      option.setAttribute('aria-selected', 'false');

      textSpan.className = 'ss-option-text';
      textSpan.textContent = text;
      option.appendChild(textSpan);
      option.appendChild(createIcon('fas fa-check ss-check'));

      if (createdInSession) {
        option.appendChild(buildUndoButton(optionId, option));
      }

      option.addEventListener('click', function () {
        selectOption(option);
      });

      return option;
    }

    function ensureOption(value, text, optionId, createdInSession) {
      var existingOption = qsa(list, '.ss-option').filter(function (item) {
        return item.getAttribute('data-value') === String(value);
      })[0];
      var option = existingOption || buildOption(value, text, optionId, createdInSession);

      if (!existingOption) {
        list.appendChild(option);
      }

      return option;
    }

    function removeOption(value) {
      qsa(list, '.ss-option').forEach(function (option) {
        if (option.getAttribute('data-value') !== String(value)) {
          return;
        }

        if (hiddenInput.value === String(value)) {
          hiddenInput.value = '';
          triggerText.textContent = placeholder;
          triggerText.classList.add('ss-placeholder');
          hiddenInput.dispatchEvent(new Event('change', { bubbles: true }));
        }

        if (option.parentNode) {
          option.parentNode.removeChild(option);
        }
      });

      filterOptions(searchInput.value);
    }

    function saveNew() {
      var name = addInput ? addInput.value.replace(/^\s+|\s+$/g, '') : '';

      if (!name) {
        if (addInput) {
          addInput.focus();
        }
        return;
      }

      hide(addForm);
      show(savingIndicator);
      removeError();

      postJson(saveUrl, { name: name, category: category }).then(function (data) {
        var option;

        if (!data.success) {
          throw new Error(data.message || 'Server error');
        }

        document.dispatchEvent(new CustomEvent('searchableSelect:optionAdded', {
          detail: {
            category: category,
            value: data.value,
            name: data.name,
            optionId: data.optionId,
            created: data.created
          }
        }));

        option = ensureOption(data.value, data.name, data.optionId, data.created);
        selectOption(option);
      }).catch(function (error) {
        show(addForm);
        hide(savingIndicator);
        showError(error.message);
        if (addInput) {
          addInput.focus();
        }
      });
    }

    function showAddForm() {
      hide(addTrigger);
      show(addForm);
      if (addInput) {
        addInput.value = searchInput.value.replace(/^\s+|\s+$/g, '');
        addInput.focus();
        addInput.select();
      }
    }

    trigger.addEventListener('click', togglePanel);
    trigger.addEventListener('keydown', function (event) {
      if (event.key === 'Enter' || event.key === ' ') {
        event.preventDefault();
        togglePanel();
      }
    });

    searchInput.addEventListener('input', function () {
      filterOptions(searchInput.value);
    });

    qsa(list, '.ss-option').forEach(function (option) {
      option.addEventListener('click', function () {
        selectOption(option);
      });
    });

    document.addEventListener('searchableSelect:optionAdded', function (event) {
      var detail = event.detail || {};

      if (detail.category !== category) {
        return;
      }

      ensureOption(detail.value, detail.name, detail.optionId, detail.created);
      filterOptions(searchInput.value);
    });

    document.addEventListener('searchableSelect:optionRemoved', function (event) {
      var detail = event.detail || {};

      if (detail.category !== category) {
        return;
      }

      removeOption(detail.value);
    });

    if (addTrigger) {
      addTrigger.addEventListener('click', showAddForm);
    }

    if (addSaveButton) {
      addSaveButton.addEventListener('click', saveNew);
    }

    if (addCancelButton) {
      addCancelButton.addEventListener('click', resetAddForm);
    }

    if (addInput) {
      addInput.addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {
          event.preventDefault();
          saveNew();
        }

        if (event.key === 'Escape') {
          resetAddForm();
        }
      });
    }

    document.addEventListener('click', function (event) {
      if (!wrapper.contains(event.target)) {
        closePanel();
      }
    });

    document.addEventListener('keydown', function (event) {
      if (event.key === 'Escape' && wrapper.classList.contains('ss-open')) {
        closePanel();
      }
    });

    if (!hiddenInput.value) {
      triggerText.classList.add('ss-placeholder');
    }
  }

  function init(root) {
    qsa(root || document, '.ss-wrapper:not([data-ss-bound])').forEach(bind);
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function () {
      init(document);
    });
  } else {
    init(document);
  }

  return {
    init: init
  };
}());
