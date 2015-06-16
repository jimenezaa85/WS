(function () {
    QUnit.config.testTimeout = 10000;

    var okAsync = QUnit.okAsync,
        stringformat = QUnit.stringformat;

    var baseUrl = '/api/TaxReturn/',
        getMsgPrefix = function (id, rqstUrl) {
            return stringformat('Delete tax  id=\'{0}\' to \'{1}\'', id, rqstUrl);
        },
        onCallSuccess = function (msgPrefix) {
            ok(true, msgPrefix + " succeeded.");
        },
        onError = function (result, msgPrefix) {
            okAsync(false, msgPrefix +
                stringformat(' failed with status=\'{1}\': {2}.',
                    result.status, result.responseText));
        };

    var testTaxId = 5,
        pages = 2,
        size = 20,
        field = "description",
        dir = "asc",
        testUrl,
        testMsgBase



    module('Web API delete TaxReturn has excepted shape',
        {
            setup: function () {
                testUrl = stringformat('{0}{1}', baseUrl, testTaxId);
                testMsgBase = getMsgPrefix(testTaxId, testUrl);
            }
        });

    test('Lookups url should return array of Tax',
        function () {
            stop();
            deleteTestTaxReturn();
        }
    );

    // Step 1: Delete test tax 
    function deleteTestTaxReturn() {
        var msgPrefix =  testMsgBase;
        $.ajax({
            type: 'DELETE',
            url: testUrl,
            success: function (result) {
                onCallSuccess(msgPrefix);
                start();
            },
            error: function (result) { onError(result, msgPrefix); }
        });
    };
})();