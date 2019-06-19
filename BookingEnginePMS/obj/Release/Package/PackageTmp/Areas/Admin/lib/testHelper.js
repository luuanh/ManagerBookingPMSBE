
(function (context) {

    var DEFAULT_DATA_TABLE_LIMIT = 8;

    var objToString = Object.prototype.toString;
    var TYPED_ARRAY = {
        '[object Int8Array]': 1,
        '[object Uint8Array]': 1,
        '[object Uint8ClampedArray]': 1,
        '[object Int16Array]': 1,
        '[object Uint16Array]': 1,
        '[object Int32Array]': 1,
        '[object Uint32Array]': 1,
        '[object Float32Array]': 1,
        '[object Float64Array]': 1
    };

    var testHelper = {};
    testHelper.create = function (echarts, domOrId, opt) {
        var dom = getDom(domOrId);

        if (!dom) {
            return;
        }

        var title = document.createElement('div');
        var left = document.createElement('div');
        var chartContainer = document.createElement('div');
        var buttonsContainer = document.createElement('div');
        var dataTableContainer = document.createElement('div');
        var infoContainer = document.createElement('div');

        title.setAttribute('title', dom.getAttribute('id'));

        title.className = 'test-title';
        dom.className = 'test-chart-block';
        left.className = 'test-chart-block-left';
        chartContainer.className = 'test-chart';
        buttonsContainer.className = 'test-buttons';
        dataTableContainer.className = 'test-data-table';
        infoContainer.className = 'test-info';

        if (opt.info) {
            dom.className += ' test-chart-block-has-right';
            infoContainer.className += ' test-chart-block-right';
        }

        left.appendChild(buttonsContainer);
        left.appendChild(dataTableContainer);
        left.appendChild(chartContainer);
        dom.appendChild(infoContainer);
        dom.appendChild(left);
        dom.parentNode.insertBefore(title, dom);

        var chart;

        var optTitle = opt.title;
        if (optTitle) {
            if (optTitle instanceof Array) {
                optTitle = optTitle.join('\n');
            }
            title.innerHTML = '<div class="test-title-inner">'
                + testHelper.encodeHTML(optTitle).replace(/\n/g, '<br>')
                + '</div>';
        }

        if (opt.option) {
            chart = testHelper.createChart(echarts, chartContainer, opt.option, opt, opt.setOptionOpts);
        }

        var dataTables = opt.dataTables;
        if (!dataTables && opt.dataTable) {
            dataTables = [opt.dataTable];
        }

        var buttons = opt.buttons || opt.button;
        if (!(buttons instanceof Array)) {
            buttons = buttons ? [buttons] : [];
        }
        if (buttons.length) {
            for (var i = 0; i < buttons.length; i++) {
                var btnDefine = buttons[i];
                if (btnDefine) {
                    var btn = document.createElement('button');
                    btn.innerHTML = testHelper.encodeHTML(btnDefine.name || btnDefine.text || 'button');
                    btn.addEventListener('click', btnDefine.onClick || btnDefine.onclick);
                    buttonsContainer.appendChild(btn);
                }
            }
        }

        if (opt.info) {
            infoContainer.innerHTML = createObjectHTML(opt.info, opt.infoKey || 'option');
        }

        return chart;
    };
    testHelper.createChart = function (echarts, domOrId, option, opt) {
        if (typeof opt === 'number') {
            opt = {height: opt};
        }
        else {
            opt = opt || {};
        }

        var dom = getDom(domOrId);

        if (dom) {
            if (opt.width != null) {
                dom.style.width = opt.width + 'px';
            }
            if (opt.height != null) {
                dom.style.height = opt.height + 'px';
            }

            var chart = echarts.init(dom);

            if (opt.draggable) {
                window.draggable.init(dom, chart, {throttle: 70, addPlaceholder: true});
            }

            option && chart.setOption(option, {
                lazyUpdate: opt.lazyUpdate,
                notMerge: opt.notMerge
            });
            testHelper.resizable(chart);

            return chart;
        }
    };


    testHelper.resizable = function (chart) {
        if (window.attachEvent) {
            window.attachEvent('onresize', chart.resize);
        } else if (window.addEventListener) {
            window.addEventListener('resize', chart.resize, false);
        }
    };


    testHelper.encodeHTML = function (source) {
        return String(source)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    };

    var getType = testHelper.getType = function (value) {
        var type = typeof value;
        var typeStr = objToString.call(value);

        return !!TYPED_ARRAY[objToString.call(value)]
            ? 'typedArray'
            : typeof type === 'function'
            ? 'function'
            : typeStr === '[object Array]'
            ? 'array'
            : typeStr === '[object Number]'
            ? 'number'
            : typeStr === '[object Boolean]'
            ? 'boolean'
            : typeStr === '[object String]'
            ? 'string'
            : typeStr === '[object RegExp]'
            ? 'regexp'
            : typeStr === '[object Date]'
            ? 'date'
            : !!value && type === 'object'
            ? 'object'
            : null;
    };

    var getDom = testHelper.getDom = function (domOrId) {
        return getType(domOrId) === 'string' ? document.getElementById(domOrId) : domOrId;
    }
    context.testHelper = testHelper;

})(window);