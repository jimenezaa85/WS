(function () {
    QUnit.config.testTimeout = 10000;

    var okAsync = QUnit.okAsync,
        stringformat = QUnit.stringformat;

    var baseUrl = '/api/TaxReturn/Get',
        getMsgPrefix = function (id, rqstUrl) {
            return stringformat(' tax list id=\'{0}\' to \'{1}\'',id, rqstUrl);
        },
        onCallSuccess = function (msgPrefix) {
            ok(true, msgPrefix + " succeeded.");
        },
        onError = function (result, msgPrefix) {
            okAsync(false, msgPrefix +
                stringformat(' failed with status=\'{1}\': {2}.',
                    result.status, result.responseText));
        };

    var testTaxId = 3,
        pages = 2,
        size = 20,
        testUrl,
        testMsgBase



    module('Web API GET TaxReturn has excepted shape',
        {
            setup: function () {
                testUrl = stringformat('{0}?pageNumber={1}&pageSize={2}', baseUrl, pages, size);
                //testMsgBase = getMsgPrefix(testTaxReturnId, testUrl);
                testMsgBase = " Tax list";
            }
        });

    test('Lookups url should return array of Tax',
        function () {
            stop();
            getTestTaxReturn();
        }
    );

    // Step 1: Get test tax list (this fnc is re-used several times)
    function getTestTaxReturn() {
        var msgPrefix = 'GET ' + testMsgBase;
        $.ajax({
            type: 'GET',
            url: testUrl,
            success: function (result) {
                onCallSuccess(msgPrefix);
                okAsync(result != null, " returned  items: " + result.totalCount + " total items");
                

                start();
            },
            error: function (result) { onError(result, msgPrefix); }
        });
    };
})();