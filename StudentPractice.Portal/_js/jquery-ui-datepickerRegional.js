jQuery(function ($) {
    $.datepicker.regional['el'] = {
        closeText: 'Κλείσιμο',
        prevText: 'Προηγούμενος',
        nextText: 'Επόμενος',
        currentText: 'Σήμερα',
        monthNames: ['Ιανουάριος', 'Φεβρουάριος', 'Μάρτιος', 'Απρίλιος', 'Μάιος', 'Ιούνιος', 'Ιούλιος', 'Αύγουστος', 'Σεπτέμβριος', 'Οκτώβριος', 'Νοέμβριος', 'Δεκέμβριος'],
        monthNamesShort: ['Ιαν', 'Φεβ', 'Μαρ', 'Απρ', 'Μαι', 'Ιουν', 'Ιουλ', 'Αυγ', 'Σεπ', 'Οκτ', 'Νοε', 'Δεκ'],
        dayNames: ['Κυριακή', 'Δευτέρα', 'Τρίτη', 'Τετάρτη', 'Πέμπτη', 'Παρασκευή', 'Σάββατο'],
        dayNamesShort: ['Κυρ', 'Δευ', 'Τρι', 'Τετ', 'Πεμ', 'Παρ', 'Σαβ'],
        dayNamesMin: ['Κυ', 'Δε', 'Τρ', 'Τε', 'Πε', 'Πα', 'Σα'],
        weekHeader: 'Εβδ',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        showButtonPanel: true,
        yearSuffix: '',
        changeYear: true
    };

    $.datepicker.regional['en'] = {
        closeText: 'Close',
        prevText: 'Prev',
        nextText: 'Next',
        currentText: 'Today',
        monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        dayNamesShort: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        dayNamesMin: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
        weekHeader: 'Wk',
        dateFormat: 'mm/dd/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        showButtonPanel: true,
        yearSuffix: '',
        changeYear: true
    };
    $.datepicker.setDefaults($.datepicker.regional[Resources.Datepicker_Regional]);
});