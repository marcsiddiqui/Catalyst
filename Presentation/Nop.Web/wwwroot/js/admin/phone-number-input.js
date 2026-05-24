var PhoneNumberInput = (function () {
  function qsa(root, selector) {
    return root ? Array.prototype.slice.call(root.querySelectorAll(selector)) : [];
  }

  function digitsOnly(value) {
    return (value || '').replace(/\D/g, '').substring(0, 11);
  }

  function formatPhoneNumber(digits) {
    if (digits.length !== 11) {
      return '';
    }

    return digits.substring(0, 4) + '-' + digits.substring(4);
  }

  function bind(wrapper) {
    if (!wrapper || wrapper.getAttribute('data-phone-number-bound')) {
      return;
    }

    wrapper.setAttribute('data-phone-number-bound', '1');

    var hiddenInput = wrapper.querySelector('.phone-number-hidden-input');
    var error = wrapper.querySelector('.phone-number-error');
    var inputs = qsa(wrapper, '.phone-number-digit');
    var syncingHidden = false;

    if (!hiddenInput || inputs.length !== 11) {
      return;
    }

    function getDigits() {
      return inputs.map(function (input) {
        return input.value || '';
      }).join('');
    }

    function setInvalid(isInvalid) {
      wrapper.classList.toggle('phone-number-invalid', isInvalid);
      if (error) {
        error.hidden = !isInvalid;
      }
    }

    function syncHidden(showValidation) {
      var typedDigits = digitsOnly(getDigits());
      var invalid = typedDigits.length > 0 && typedDigits.length < 11;

      syncingHidden = true;
      hiddenInput.value = formatPhoneNumber(typedDigits);
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
    qsa(root || document, '.phone-number-input:not([data-phone-number-bound])').forEach(bind);
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
