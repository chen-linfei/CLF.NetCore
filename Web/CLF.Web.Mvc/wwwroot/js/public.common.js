// 防止请求伪造
function addAntiForgeryToken(data) {
    if (!data) {
        data = {};
    }
    var tokenInput = $('input[name=__RequestVerificationToken]');
    if (tokenInput.length) {
        data.__RequestVerificationToken = tokenInput.val();
    }
    return data;
};

Array.prototype.contains = function (item) {
    return RegExp("\\b" + item + "\\b").test(this);
};
Array.prototype.remove = function (index) {
    if (isNaN(index) || index > this.length) {
        return false;
    }
    this.splice(index, 1);
};

Array.prototype.indexOf = function (object) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == object) {
            return i;
        }
    }
    return -1;
};

Array.prototype.removeChild = function (object) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == object) {
            this.remove(i);
            break;
        }
    }
};

(function ($, undefined) {
    $.extend({
        layer: function () {
            return layer;
        }
    });

    $.layer.showErrorMessage = function (message, title, timeout) {
        layer.alert(
            message,
            {
                title: title || '系统提示',
                skin: 'layui-layer-lan',
                icon: 2,
                btn: null,
                time: timeout || 2000
            });
    };

    /**
     * layer alert封装
     * @param message 
     * @param title
     * @param timeout
     * @param fnCancel 关闭按钮时回调方法
     * @param fnEnd 销毁时回调方法
     */
    $.layer.showSuccessMessage = function (message, title, timeout, fnCancel, fnEnd) {
        layer.alert(
            message, {
                title: title || '系统提示',
                skin: 'layui-layer-lan',
                icon: 1,
                btn: null,
                time: timeout || 1500,
                cancel: function () {
                    fnCancel();
                },
                end: function () {
                    fnEnd();
                }
            });
    };
})(jQuery);

$.fn.extend({
    jqDataTable: function (options) {
        var defaultSettings = {};
        defaultSettings.filter = false;
        defaultSettings.sort = false;
        defaultSettings.serverSide = true;
        defaultSettings.pagingType = 'full_numbers';//分页显示信息（首页，末页，上一页，下一页）
        defaultSettings.processing = true;//是否显示正在处理
        //defaultSettings.pageLength = 10;//每页的行数
        //defaultSettings.lengthChange = true;//是否显示一个每页长度的选择条（需要分页器支持）
        defaultSettings.autoWidth = true;//是否宽度自适应
        defaultSettings.language = {
            processing: '正在加载中......',
            lengthMenu: '显示 _MENU_ 条记录',
            zeroRecords: '对不起，查询不到相关数据！',
            emptyTable: '表中无数据存在！',
            info: '显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录',
            infoFiltered: '数据表中共为 _MAX_ 条记录',
            search: '搜索',
            paginate: {
                sFirst: '首页',
                sPrevious: '上一页',
                sNext: '下一页',
                sLast: '末页'
            },
            select: {
                rows: {
                    _: ' 已选择 %d行'
                }
            }
        };
        $.extend(options, defaultSettings);

        //添加checkbox列
        var isSelect = options.select;
        if (isSelect && isSelect.style == 'multi') {
            var columns = options.columns;
            var column = {
                title: '<input id="materCheckbox" class="checkboxGroups"  type="checkbox"/>',
                data: null,
                width: '2%',
                render: function () {
                    return '<input class="checkboxGroups"  type="checkbox"/>'
                }
            }
            options.columns.splice(0, 0, column);
        }

        return $(this).DataTable(options);
    },
    //到页面顶部
    backTop: function () {
        var backBtn = this;
        var position = 1000;
        var speed = 900;

        $(document).scroll(function () {
            var pos = $(window).scrollTop();
            if (pos >= position) {
                backBtn.fadeIn(speed);
            } else {
                backBtn.fadeOut(speed);
            }
        });

        backBtn.click(function () {
            $("html, body").animate({ scrollTop: 0 }, 900);
        });
    }
});

function initTableCheckbox(table) {
    var masterCheckbox = table.find('#materCheckbox');

    //全选
    masterCheckbox.click(function () {
        $('.checkboxGroups').prop('checked', $(this).is(':checked')).change();
    });

    //单选
    table.on('change', 'input[type=checkbox][id!=materCheckbox][class=checkboxGroups]', function (e) {
        var $check = $(this);
        if ($check.is(':checked')) {
            table.DataTable().rows($check.parent().parent()).select();
        } else {
            table.DataTable().rows($check.parent().parent()).deselect();
        }
        updateMasterCheckbox();
    });

    //选择行
    table.DataTable().on('select', function (e, dt, type, indexes) {
        var checkbox = $(this).find('input[type=checkbox][id!=materCheckbox][class=checkboxGroups]').eq(indexes);
        checkbox.prop('checked', true);
        updateMasterCheckbox();
    }).on('deselect', function (e, dt, type, indexes) {
        var checkbox = $(this).find('input[type=checkbox][id!=materCheckbox][class=checkboxGroups]').eq(indexes);
        checkbox.prop('checked', false);
        updateMasterCheckbox();
    });

    function updateMasterCheckbox() {
        var numChkBoxes = $('input[type=checkbox][id!=materCheckbox][class=checkboxGroups]').length;
        var numChkBoxesChecked = $('input[type=checkbox][id!=materCheckbox][class=checkboxGroups]:checked').length;
        $('#materCheckbox').prop('checked', numChkBoxes == numChkBoxesChecked && numChkBoxes > 0);
    }
}

