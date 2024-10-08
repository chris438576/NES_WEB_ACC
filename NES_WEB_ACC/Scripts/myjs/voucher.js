//#region 功能函式
// 自定義doajax
function doajax(type, url, data, successCallback, errorCallback) {
    $.ajax({
        type: type,
        url: url,
        data: data,
        success: function (response) {
            if (typeof successCallback === 'function') {
                successCallback(response);
            }
        },
        error: function (xhr, status, errorThrown) {
            if (typeof errorCallback === 'function') {
                errorCallback(xhr, status, errorThrown);
            }
        }
    });
}

// PageReload
function pageReload(billno, msg) {
    var url = new URL(window.location.href);
    url.searchParams.set('billno', billno);
    url.searchParams.set('msg', msg);
    window.location.href = url.toString();
}

// Auto Select Row
function selectRow(billno) {
    var rows = maindg.datagrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].BillNo === billno) {
            maindg.datagrid('selectRow', i);
            return;
        }
    }
}

// 表頭、進階資料、借貸平衡填值
function tableHeadData(row) {
    $('.colBillDate').textbox('setValue', row.BillDate);
    $('.colDocSubType').textbox('setValue', row.DocSubType);
    let colDocSubTypeName;
    objdata(objDocSubType, 'read', function (data) {
        if (data.data) {
            for (let item of data.data) {
                if (item.DocSubType === row.DocSubType) {
                    colDocSubTypeName = item.DocSubTypeName
                }
            }
        } else {
            console.log('DocSubType Set Error');
            colDocSubTypeName = null;
        }
    });
    $('.colDocSubTypeName').textbox('setValue', colDocSubTypeName);
    $('.colCompNo').textbox('setValue', row.CompNo);
    $('.colCompAbbr').textbox('setValue', row.CompAbbr);
    $('.colVoucherType').textbox('setValue', row.VoucherType);
    let colVoucherName;
    objdata(objVoucherType, 'read', function (data) {
        if (data.data) {
            for (let item of data.data) {
                if (item.VoucherType == row.VoucherType) {
                    colVoucherName = item.VoucherName
                }
            }
        } else {
            console.log('VoucherType Set Error');
            colVoucherName = null;
        }
    });
    $('.colVoucherNameC').textbox('setValue', colVoucherName);
    $('.colEmpNo').textbox('setValue', row.EmpNo);
    $('.colEmpNameC').textbox('setValue', row.EmpNameC);
    $('.colDeptNo').textbox('setValue', row.DeptNo);
    $('.colDeptName').textbox('setValue', row.DeptName);
    $('.colBillNo').textbox('setValue', row.BillNo);
    $('.colCurrencyNo').textbox('setValue', row.CurrencyNo);
    $('.colCurrencySt').textbox('setValue', row.CurrencySt);
    $('.colRate1').textbox('setValue', row.Rate1);
    /* $('.colCurrency2').textbox('setValue',row.VoucherNameC);*/
    /* $('.colRate2').textbox('setValue', row.Rate2);*/
    $('.colRemark').textbox('setValue', row.Remark);
    //進階資料
    $('.colLedger').textbox('setValue', '帳簿1');
    $('.colCompNo').textbox('setValue', row.CompNo);
    $('.colCompAbbr').textbox('setValue', row.CompAbbr);
    $('.colAccDocType').textbox('setValue', row.AccDocType);
    $('.colSourceDocSubTypeName').textbox('setValue', row.SourceDocSubTypeName);
    $('.colActivityType').textbox('setValue', row.ActivityType);
    $('.colSourceNo').textbox('setValue', row.SourceNo);
    //借貸平衡
    $('.colCurrencyD').textbox('setValue', row.CurrencySt);
    $('.colCurrencyC').textbox('setValue', row.CurrencySt);
    $('.colCurrency').textbox('setValue', row.CurrencySt);
}

// obj資料操作
function objdata(objtype, action, callback) {
    switch (objtype.name) {
        case 'AccNo':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetEditTableCode';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;
        case 'CurrencyNo':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetEditTableCode3';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });

                }
            }
            break;
        case 'AccDept':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetEditTableCode5';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;
        case 'TargetNo0':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetEditTableCode4?type=15';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;
        case 'TargetNo1':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetEditTableCode4?type=14';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;
        case 'TargetNo2':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetEditTableCode4?type=12';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;
        case 'TargetNo3':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetEditTableCode4?type=65';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;
        case 'PayDept':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetEditTableCode6';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;
        case 'TargetType':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetEditTableCode2';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;
        case 'VoucherType':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetAddInfoCode3';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;
        case 'DocSubType':
            if (action === 'clear') {
                objtype.data = null;
            } else if (action === 'read') {
                if (objtype.data != null) {
                    if (callback) callback(objtype);
                } else {
                    actionurl = defaultUrl + controllerurl + functionurl + 'GetAddInfoCode';
                    doajax("GET", actionurl, null, function (result) {
                        if (result.success) {
                            objtype.data = result.data;
                        } else {
                            $.messager.alert(msglang.Title2, msglang.MsgErrCode + result.code);
                            objtype.data = null;
                            if (result.code == 'C0003') {
                                console.log("C0003: [AccNo] Not Found");
                            }
                            if (result.code == 'C0004') {
                                console.log(result.err);
                            }
                        }
                        if (callback) callback(objtype);
                    });
                }
            }
            break;        
    }
}
//#endregion


//#region 格式化
// 日期轉換
function formatDate(dateString) {
    const regex = /\/Date\((\d+)\)\//;
    const match = dateString.match(regex);
    const timestamp = match ? parseInt(match[1], 10) : 0;
    const date = new Date(timestamp);

    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);

    return `${year}/${month}/${day}`;
}

// itemdg欄位Function
function formatTargetType(value) {
    switch (String(value)) {
        case '15':
            return '廠商';
        case '14':
            return '客戶';
        case '12':
            return '員工';
        case '65':
            return '銀行';
        default:
            return value; // 若不在上述範圍內，則回傳原值
    }
}
//#endregion