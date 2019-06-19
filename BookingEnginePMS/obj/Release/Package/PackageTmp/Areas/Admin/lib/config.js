
(function () {

    var baseUrlChart = window.AMD_BASE_URL || '/Areas/admin/lib/';

	var ecDistPath = 'echarts';

    if (typeof requireChart !== 'undefined') {
        requireChart.config({
            baseUrl: baseUrlChart,
            paths: {
                'echarts': ecDistPath,
            }
        });
    }

    function printBundleVersion(bundleIds, bundles) {
        var content = [];
        for (var i = 0; i < bundleIds.length; i++) {
            var bundle = bundles[i];
            var bundleVersion = bundle && bundle.bundleVersion;
            if (bundleVersion) {
                var date = new Date(+bundleVersion);
                // Check whether timestamp.
                if (!isNaN(+date)) {
                    bundleVersion = '<span style="color:yellow">'
                        + pad(date.getHours(), 2) + ':'
                        + pad(date.getMinutes(), 2) + ':'
                        + pad(date.getSeconds(), 2) + '.' + pad(date.getMilliseconds(), 3)
                        + '</span>';
                }
                else {
                    bundleVersion = encodeHTML(bundleVersion);
                }
                content.push(encodeHTML(bundleIds[i]) + '.js: ' + bundleVersion);
            }
        }

        var domId = 'ec-test-bundle-version';
        var dom = document.getElementById(domId);
        if (!dom) {
            dom = document.createElement('div');
            dom.setAttribute('id', domId);
            dom.style.cssText = [
                'background: rgb(52,56,64)',
                'color: rgb(215,215,215)',
                'position: fixed',
                'right: 0',
                'top: 0',
                'font-size: 10px',
                'padding: 1px 3px 1px 3px',
                'border-bottom-left-radius: 3px'
            ].join(';');
            document.body.appendChild(dom);
        }
        dom.innerHTML += content.join('');
    }
})();