/*

(function () {
    var oldLog = console.log;
    console.log = function (message) {
        if (typeof control != 'undefined') {
            control.javascriptLog(message);
        } else if (typeof nav != 'undefined') {
            nav.javascriptLog(message);
        } else if (typeof expWizard != 'undefined') {
            expWizard.javascriptLog(message);
        }
        oldLog.apply(console, arguments);
    };

    
    var oldErr = console.err;
    console.err = function (message) {
        if (typeof control != 'undefined') {
            control.javascriptLog(message);
        } else if (typeof nav != 'undefined') {
            nav.javascriptLog(message);
        } else if (typeof expWizard != 'undefined') {
            expWizard.javascriptLog(message);
        }
        oldErr.apply(console, arguments);
    };
    
    var oldInfo = console.info;
    console.info = function (message) {
        if (typeof control != 'undefined') {
            control.javascriptLog(message);
        } else if (typeof nav != 'undefined') {
            nav.javascriptLog(message);
        } else if (typeof expWizard != 'undefined') {
            expWizard.javascriptLog(message);
        }
        oldInfo.apply(console, arguments);
    };

    var oldAlert = alert;
    alert = function (message) {
        if (typeof control != 'undefined') {
            control.javascriptLog(message);
        } else if (typeof nav != 'undefined') {
            nav.javascriptLog(message);
        } else if (typeof expWizard != 'undefined') {
            expWizard.javascriptLog(message);
        }
        oldAlert.apply(this, arguments);
    };

    
    var oldWindowError = window.onerror;
    window.onerror = function (message, file, line) {
        if (typeof control != 'undefined') {
            control.javascriptLog(file + ':' + line + '\n\n' + message);
        } else if (typeof nav != 'undefined') {
            nav.javascriptLog(file + ':' + line + '\n\n' + message);
        } else if (typeof expWizard != 'undefined') {
            expWizard.javascriptLog(file + ':' + line + '\n\n' + message);
        }
        oldWindowError.apply(window, arguments);
    };
    
})();

*/