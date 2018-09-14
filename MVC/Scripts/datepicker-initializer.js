/**
 * Initializes bootstrap datepicker module for every .datepicker element.
 * datepickers default format is: "dd-MMM-yyyy". Optionally you can provide a different format parameter.
 * This is meant to be flexible if in every view is required different date formating.
 */
function datePickerInit(dateformat = "dd-MMM-yyyy") {

    var type = typeof dateformat;

    if (type !== 'string') {
        console.error(`${type} is not acceptable as date format. datepickers will be initialized with the default format: "dd-MMM-yyyy"`)
        dateformat = "dd-MMM-yyyy"
    }

    $('.datepicker').datepicker({

        autoclose: true,

        // include today button
        todayBtn: "linked",

        // convert month formating for date picker: https://bootstrap-datepicker.readthedocs.io/en/latest/options.html#format
        // to comply with C# date formating: https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1
        // C# => JS
        // M => m
        // MM => mm
        // MMM => M
        // MMMM => MM
        format: dateformat
            .replace(/M/g, 'm')
            .replace(/mmmm/g, 'MM')
            .replace(/mmm/g, 'M')
    });
}

/**
 * Connects 2 datepickers to implement a date range between them.
 * @param {any} from the starting datepicker. This will define the start date of the range.
 * @param {any} to the ending date picker. This will define the end date of the range
 */
function linkDatePickers(from, to) {

    // on date change (set start date)
    from.on("changeDate", function (e) {
        // Get the date from the linked 1st.
        // This is needed because if that date is out of the incoming range, datepicker('getDate') will return null after the range modification.
        // Basically, the range modification invalidates the datepicker's value if that is now invalid (out of range).
        // However, it does not modify the hosting input's value. This leads to confusion because the user can see the date in the input, but in reality is null.
        // To avoid such occurencies, we get the date before any range modification, and then handle accordingly if that will be invalid.
        var curToDate = to.datepicker('getDate');
        // modify the range
        to.datepicker('setStartDate', e.date);
        // check if the date
        if (curToDate && curToDate.getTime() < e.date.getTime()) {
            to.datepicker('setDate', e.date);
        }
    });

    // same goes to set the end date
    to.on("changeDate", function (e) {
        var curFromDate = from.datepicker('getDate');
        from.datepicker('setEndDate', e.date);
        if (curFromDate && curFromDate.getTime() > e.date.getTime()) {
            from.datepicker('setDate', e.date);
        }
    });

    // finally, apply this rules to the current values.

    var startDate = from.datepicker('getDate');
    if (startDate) {
        to.datepicker('setStartDate', startDate);
    }

    var endDate = to.datepicker('getDate');
    if (endDate) {
        from.datepicker('setEndDate', endDate);
    }
}
