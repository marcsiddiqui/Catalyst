var CnicInput = (function () {
  function qsa(root, selector) {
    return root ? Array.prototype.slice.call(root.querySelectorAll(selector)) : [];
  }

  function digitsOnly(value) {
    return (value || '').replace(/\D/g, '').substring(0, 13);
  }

  function formatCnic(digits) {
    if (digits.length !== 13) {
      return '';
    }

    return digits.substring(0, 5) + '-' + digits.substring(5, 12) + '-' + digits.substring(12);
  }

  function bind(wrapper) {
    if (!wrapper || wrapper.getAttribute('data-cnic-bound')) {
      return;
    }

    wrapper.setAttribute('data-cnic-bound', '1');

    var hiddenInput = wrapper.querySelector('.cnic-hidden-input');
    var error = wrapper.querySelector('.cnic-error');
    var inputs = qsa(wrapper, '.cnic-digit');
    var syncingHidden = false;

    if (!hiddenInput || inputs.length !== 13) {
      return;
    }

    function getDigits() {
      return inputs.map(function (input) {
        return input.value || '';
      }).join('');
    }

    function setInvalid(isInvalid) {
      wrapper.classList.toggle('cnic-invalid', isInvalid);
      if (error) {
        error.hidden = !isInvalid;
      }
    }

    function syncHidden(showValidation) {
      var digits = getDigits();
      var typedDigits = digitsOnly(digits);
      var invalid = typedDigits.length > 0 && typedDigits.length < 13;

      syncingHidden = true;
      hiddenInput.value = formatCnic(typedDigits);
      hiddenInput.dispatchEvent(new Event('change', { bubbles: true }));
      syncingHidden = false;

      if (showValidation || !invalid) {
        setInvalid(invalid);
      }
    }

    function fillFromDigits(digits, startIndex) {
      var currentIndex = startIndex || 0;
      var values = digitsOnly(digits);

      values.split('').forEach(function (digit) {
        if (currentIndex < inputs.length) {
          inputs[currentIndex].value = digit;
          currentIndex += 1;
        }
      });

      syncHidden(false);

      if (currentIndex < inputs.length) {
        inputs[currentIndex].focus();
      } else {
        inputs[inputs.length - 1].focus();
      }
    }

    inputs.forEach(function (input, index) {
      input.addEventListener('keydown', function (event) {
        if (event.key === 'Backspace') {
          event.preventDefault();

          if (input.value) {
            input.value = '';
            syncHidden(false);
            return;
          }

          if (index > 0) {
            inputs[index - 1].value = '';
            inputs[index - 1].focus();
            syncHidden(false);
          }

          return;
        }

        if (event.key === 'Delete') {
          event.preventDefault();
          input.value = '';
          syncHidden(false);
          return;
        }

        if (event.key === 'ArrowLeft' && index > 0) {
          event.preventDefault();
          inputs[index - 1].focus();
          return;
        }

        if (event.key === 'ArrowRight' && index < inputs.length - 1) {
          event.preventDefault();
          inputs[index + 1].focus();
          return;
        }

        if (event.key.length === 1 && !/^\d$/.test(event.key)) {
          event.preventDefault();
        }
      });

      input.addEventListener('input', function () {
        var value = digitsOnly(input.value);

        if (value.length > 1) {
          input.value = '';
          fillFromDigits(value, index);
          return;
        }

        input.value = value;
        syncHidden(false);

        if (value && index < inputs.length - 1) {
          inputs[index + 1].focus();
        }
      });

      input.addEventListener('paste', function (event) {
        event.preventDefault();
        fillFromDigits(event.clipboardData.getData('text'), index);
      });

      input.addEventListener('blur', function () {
        syncHidden(true);
      });
    });

    hiddenInput.addEventListener('change', function () {
      if (syncingHidden) {
        return;
      }

      var digits = digitsOnly(hiddenInput.value);

      inputs.forEach(function (input, index) {
        input.value = digits[index] || '';
      });

      setInvalid(false);
    });

    syncHidden(false);
  }

  function init(root) {
    qsa(root || document, '.cnic-input:not([data-cnic-bound])').forEach(bind);
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
