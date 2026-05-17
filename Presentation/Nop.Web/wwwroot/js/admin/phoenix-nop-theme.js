/*
  Phoenix admin theme integration start.
  Vanilla theme switcher inspired by Phoenix. No Phoenix JS or Bootstrap 5 dependency.
  Phoenix admin theme integration end.
*/
(function () {
    'use strict';

    var storageKey = 'nopAdminPhoenixTheme';
    var allowedThemes = {
        light: true,
        dark: true,
        auto: true
    };
    var systemThemeQuery = window.matchMedia ? window.matchMedia('(prefers-color-scheme: dark)') : null;

    function getStoredTheme() {
        try {
            var storedTheme = localStorage.getItem(storageKey);
            return allowedThemes[storedTheme] ? storedTheme : 'light';
        } catch (error) {
            return 'light';
        }
    }

    function storeTheme(theme) {
        try {
            localStorage.setItem(storageKey, theme);
        } catch (error) {
            // Ignore storage failures so admin behavior remains unaffected.
        }
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

    function applyTheme(theme, shouldStore) {
        var selectedTheme = allowedThemes[theme] ? theme : 'light';
        var resolvedTheme = getResolvedTheme(selectedTheme);

        if (shouldStore) {
            storeTheme(selectedTheme);
        }

        document.documentElement.setAttribute('data-bs-theme', resolvedTheme);
        setBodyThemeClass(resolvedTheme);
        updateControls(selectedTheme, resolvedTheme);
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
        if (getStoredTheme() === 'auto') {
            applyTheme('auto', false);
        }
    }

    function initThemeControls() {
        document.addEventListener('click', handleThemeControlClick);
        document.addEventListener('change', handleThemeControlChange);
        applyTheme(getStoredTheme(), false);

        if (systemThemeQuery) {
            if (systemThemeQuery.addEventListener) {
                systemThemeQuery.addEventListener('change', handleSystemThemeChange);
            } else if (systemThemeQuery.addListener) {
                systemThemeQuery.addListener(handleSystemThemeChange);
            }
        }
    }

    applyTheme(getStoredTheme(), false);

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initThemeControls);
    } else {
        initThemeControls();
    }
})();
