(function ($) {
    $.fn.myDatebox = function (options) {
        var settings = $.extend({
            culture: 'en',
            dateboxSelector: this
        }, options);

        var langToday, langClose, langWeeks, langMonths;

        switch (settings.culture) {
            case 'en':
                langToday = 'Today';
                langClose = 'Close';
                langWeeks = ['S', 'M', 'T', 'W', 'T', 'F', 'S'];
                langMonths = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
                break;
            case 'zh-TW':
                langToday = '今天';
                langClose = '關閉';
                langWeeks = ['日', '一', '二', '三', '四', '五', '六'];
                langMonths = ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'];
                break;
            case 'es-MX':
                langToday = 'Hoy';
                langClose = 'Cerca';
                langWeeks = ['D', 'L', 'M', 'M', 'J', 'V', 'S'];
                langMonths = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];
                break;
            default:
                langToday = 'Today';
                langClose = 'Close';
                langWeeks = ['S', 'M', 'T', 'W', 'T', 'F', 'S'];
                langMonths = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
                break;
        }

        function myformatter(date) {
            if (!date) return '';
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return y + '/' + (m < 10 ? ('0' + m) : m) + '/' + (d < 10 ? ('0' + d) : d);
        }

        function myparser(s) {
            if (!s) return new Date();
            var ss = s.split('/');
            var y = parseInt(ss[0], 10);
            var m = parseInt(ss[1], 10);
            var d = parseInt(ss[2], 10);
            if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                return new Date(y, m - 1, d);
            } else {
                return new Date();
            }
        }

        function mycalendar(target) {
            var opts = $(target).datebox('options');
            $(target).datebox('calendar').calendar({
                validator: function (date) {
                    return true;
                },
                firstDay: opts.firstDay,
                onSelect: function (date) {
                    $(target).datebox('hidePanel');
                    var value = opts.formatter.call(target, date);
                    $(target).datebox('setText', value).datebox('setValue', value);
                }
            });
        }

        return this.each(function () {
            $(this).datebox({
                formatter: myformatter,
                parser: myparser,
                calendar: mycalendar,
                closeText: langClose,
                currentText: langToday,
                onShowPanel: function () {
                    var c = $(this).datebox('calendar');
                    c.calendar({
                        weeks: langWeeks,
                        months: langMonths,
                    });
                }
            });
        });
    };
}(jQuery));
