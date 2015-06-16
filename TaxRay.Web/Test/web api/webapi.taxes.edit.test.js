(function () {
    QUnit.config.testTimeout = 10000;

    var okAsync = QUnit.okAsync,
        stringformat = QUnit.stringformat;

    var baseUrl = '/api/TaxReturn/',
        getMsgPrefix = function (id, rqstUrl) {
            return stringformat(' id=\{0}\ to \'{1}\'', id, rqstUrl);
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
        origClient,
        newClient,
        testTaxReturn,
        testUrl,
        testMsgBase

    module('Web API Edit  TaxReturn has excepted shape',
        {
            setup: function () {
                testUrl = stringformat('{0}{1}', baseUrl, testTaxId);
                testMsgBase = getMsgPrefix(testTaxId, testUrl);
            }
        });

    test('Lookups url should return array of Tax',
        function () {
            stop();
            getTestTaxReturn(editTestTaxReturn);
        }
    );


    //Step 1: Get test TaxReturn
    function getTestTaxReturn(succeed) {
        var msgPrefix = 'GET' + testMsgBase;
        $.ajax({
            type: 'GET',
            url: testUrl,
            success: function (result) {
                onCallSuccess(msgPrefix);
                okAsync(result.id === testTaxId, "returned key matches testTaxReturn Id.");
                if (typeof succeed !== 'function') {
                    start(); // no 'succeed' callback; end of the line
                    return;
                } else {
                    succeed(result);
                }
            },
            error: function (result) { onError(result, msgPrefix); }
        });
    }

    // Step 2: Change test Tax Return 
    function editTestTaxReturn(TaxReturn) {
        testTaxReturn = TaxReturn;
        origClient = testTaxReturn.client;
        newClient = origClient === "Toyota" ? "Toyota changed" : "Toyota";
        testTaxReturn.client = newClient;
        
        testTaxReturn.assignedTo = { id: testTaxReturn.assignedToId, userName: testTaxReturn.userNameAssignedTo }
        testTaxReturn.CreatedBy = { id: testTaxReturn.createdById, userName: testTaxReturn.userNameCreatedBy }
        
        var year, month, day;
        year = TaxReturn.dueDate.substring(6);
        month = TaxReturn.dueDate.substring(3, 5)-1;
        day = TaxReturn.dueDate.substring(0,2);
        testTaxReturn.dueDate = new Date(year, month,day).toISOString();
        
        var msgPrefix = ' PUT (change) ' + testMsgBase;
        var data = testTaxReturn;

        $.ajax({
            type: 'PUT',
            url: testUrl,
            data: data,
            dataType:'json',
            success: function (result) {
                onCallSuccess(msgPrefix);
                getTestTaxReturn(confirmUpdate);
            },
            error: function (result) { onError(result, msgPrefix); }
        });
    };

    //Step 3 Confirm test TaxReturn updated, then call restore
    function confirmUpdate(confirmUpdated)
    {
        okAsync(confirmUpdated.client === newClient, "test taxReturn's client was updated ");
        restoreTestTaxReturn();
    }

    //Step 4 Restore orig test taxReturn in db
    function restoreTestTaxReturn() {
        testTaxReturn.client = origClient;
        var msgPrefix = 'PUT (restore)' + testMsgBase,
            data = testTaxReturn;

        $.ajax({
            type: 'PUT',
            url: baseUrl,
            data: data,
            dataType: 'json',
            success: function () {
                getTestTaxReturn(confirmRestored);
            },
            error: function (result) { onError(result, msgPrefix); }
        });
    }

    //Step 5: Confirm test TaxReturn was restored
    function confirmRestored(taxReturn) {
        okAsync(taxReturn.client === origClient, "test taxReturn's Client was restored ");
        start();
    };

})();