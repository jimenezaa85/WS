(function () {
    QUnit.config.testTimeout = 10000;

    var okAsync = QUnit.okAsync,
        stringformat = QUnit.stringformat;

    var baseUrl = '/api/TaxReturn/Assign',
        getMsgPrefix = function (id, rqstUrl, assignToId) {
            return stringformat('Assign To tax  id=\{0}\ userAssignToId={2} to \'{1}\'', id, rqstUrl,assignToId);
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
        userCreatedById = 1,
        assignToId=3,
        pages = 2,
        size = 20,
        field = "description",
        dir = "asc",
        testUrl,
        testMsgBase



    module('Web API assign To TaxReturn has excepted shape',
        {
            setup: function () {
                testUrl = stringformat('{0}?taxId={1}&currentId={2}&assignUserId={3}', baseUrl, testTaxId, userCreatedById, userCreatedById);
                testMsgBase = getMsgPrefix(testTaxId, testUrl,userCreatedById);
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
            type: 'PUT',
            url: testUrl,
            success: function (result) {
                onCallSuccess(msgPrefix);
                start();
            },
            error: function (result) { onError(result, msgPrefix); }
        });
    };
})();