/*
  Phoenix admin theme integration start.
  Vanilla theme switcher inspired by Phoenix. No Phoenix JS or Bootstrap 5 dependency.
  Phoenix admin theme integration end.
*/
(function () {
    'use strict';

    var allowedThemes = {
        light: true,
        dark: true,
        auto: true
    };
    var systemThemeQuery = window.matchMedia ? window.matchMedia('(prefers-color-scheme: dark)') : null;
    var currentSelectedTheme = getInitialTheme();

    function getInitialTheme() {
        var configuredTheme = window.nopAdminPhoenixTheme && window.nopAdminPhoenixTheme.selectedTheme;
        var documentTheme = document.documentElement.getAttribute('data-nop-admin-theme-choice');
        var theme = configuredTheme || documentTheme;

        return allowedThemes[theme] ? theme : 'light';
    }

    function getSaveUrl() {
        var switchElement = document.querySelector('[data-nop-theme-save-url]');
        var configuredUrl = window.nopAdminPhoenixTheme && window.nopAdminPhoenixTheme.saveUrl;

        return switchElement && switchElement.getAttribute('data-nop-theme-save-url') || configuredUrl || '';
    }

    function getAntiForgeryToken() {
        var tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');

        return tokenInput ? tokenInput.value : '';
    }

    function getResolvedTheme(theme) {
        if (theme === 'auto') {
            return systemThemeQuery && systemThemeQuery.matches ? 'dark' : 'light';
        }

        return theme;
    }

    function setBodyThemeClass(resolvedTheme) {
        if (!document.body) {
            return;
        }

        document.body.classList.toggle('admin-theme-light', resolvedTheme === 'light');
        document.body.classList.toggle('admin-theme-dark', resolvedTheme === 'dark');
    }

    function updateControls(selectedTheme, resolvedTheme) {
        var controls = document.querySelectorAll('[data-nop-theme-control]');
        var toggleIcons = document.querySelectorAll('[data-nop-theme-toggle-icon]');

        controls.forEach(function (control) {
            var controlTheme = control.getAttribute('data-nop-theme-control') || control.value;
            var isActive = controlTheme === selectedTheme;

            control.classList.toggle('active', isActive);
            control.setAttribute('aria-pressed', isActive ? 'true' : 'false');

            if ('checked' in control) {
                control.checked = isActive;
            }
        });

        toggleIcons.forEach(function (icon) {
            var iconTheme = icon.getAttribute('data-nop-theme-toggle-icon');
            icon.style.display = iconTheme === selectedTheme ? '' : 'none';
        });

        document.documentElement.setAttribute('data-nop-admin-theme-choice', selectedTheme);
        document.documentElement.setAttribute('data-nop-admin-theme-resolved', resolvedTheme);
    }

    function persistTheme(theme) {
        var saveUrl = getSaveUrl();

        if (!saveUrl) {
            return;
        }

        var body = new URLSearchParams();
        body.append('value', theme);

        var token = getAntiForgeryToken();
        if (token) {
            body.append('__RequestVerificationToken', token);
        }

        fetch(saveUrl, {
            method: 'POST',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
            },
            body: body.toString()
        }).then(function (response) {
            if (!response.ok) {
                throw new Error('Failed to save admin theme preference.');
            }
        }).catch(function (error) {
            if (window.console && window.console.warn) {
                window.console.warn(error.message || error);
            }
        });
    }

    function applyTheme(theme, shouldPersist) {
        var selectedTheme = allowedThemes[theme] ? theme : 'light';
        var resolvedTheme = getResolvedTheme(selectedTheme);

        currentSelectedTheme = selectedTheme;

        document.documentElement.setAttribute('data-bs-theme', resolvedTheme);
        setBodyThemeClass(resolvedTheme);
        updateControls(selectedTheme, resolvedTheme);

        if (shouldPersist) {
            persistTheme(selectedTheme);
        }
    }

    function handleThemeControlClick(event) {
        var control = event.target.closest('[data-nop-theme-control]');

        if (!control) {
            return;
        }

        event.preventDefault();
        applyTheme(control.getAttribute('data-nop-theme-control') || control.value, true);
    }

    function handleThemeControlChange(event) {
        var control = event.target.closest('[data-nop-theme-control]');

        if (!control) {
            return;
        }

        applyTheme(control.getAttribute('data-nop-theme-control') || control.value, true);
    }

    function handleSystemThemeChange() {
        if (currentSelectedTheme === 'auto') {
            applyTheme('auto', false);
        }
    }

    function initThemeControls() {
        document.addEventListener('click', handleThemeControlClick);
        document.addEventListener('change', handleThemeControlChange);
        applyTheme(currentSelectedTheme, false);

        if (systemThemeQuery) {
            if (systemThemeQuery.addEventListener) {
                systemThemeQuery.addEventListener('change', handleSystemThemeChange);
            } else if (systemThemeQuery.addListener) {
                systemThemeQuery.addListener(handleSystemThemeChange);
            }
        }
    }

    applyTheme(currentSelectedTheme, false);

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initThemeControls);
    } else {
        initThemeControls();
    }
})();
