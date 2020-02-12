﻿layui.define('form', function (exports) {
    var $ = layui.$
        , layer = layui.layer
        , laytpl = layui.laytpl
        , setter = layui.setter
        , view = layui.view
        , admin = layui.admin
        , form = layui.form;

    var $body = $('body');
    var element = $('#wizard');
    function next(e) {

        // If we clicked the last then dont activate this
        if (element.hasClass('last')) {
            return false;
        }

        if ($settings.onNext && typeof $settings.onNext === 'function' && $settings.onNext($activeTab, $navigation, obj.nextIndex()) === false) {
            return false;
        }

        // Did we click the last button
        $index = obj.nextIndex();
        if ($index > obj.navigationLength()) {
        } else {
            $navigation.find(baseItemSelector + ':eq(' + $index + ') a').tab('show');
        }
    }

    var $validator = $('.wizard-card form').validate({
        rules: {
            firstname: {
                required: true,
                minlength: 3
            },
            lastname: {
                required: true,
                minlength: 3
            },
            email: {
                required: true,
                minlength: 3,
            }
        },

        errorPlacement: function (error, element) {
            $(element).parent('div').addClass('has-error');
        }
    });

    form.on('submit(go)', function (data) {
        $(this).addClass('btn-next');
        layer.msg('1');
        console.log(data.elem) //被执行事件的元素DOM对象，一般为button对象
        console.log(data.form) //被执行提交的form对象，一般在存在form标签时才会返回
        console.log(data.field) //当前容器的全部表单字段，名值对形式：{name: value}
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });

    form.on('submit(createorder)', function (obj) {
        $.ajax({
            url: "/User/CreateOrder",
            type: "post",
            data: { data: JSON.stringify(obj.field) },
            success: function (res) {
                res = eval('(' + res + ')');
                if (res.code == 200) {
                    var html = '<div style="font-size:15px;">支付宝扫码支付<strong id="money">0.1</strong>元</div><br /><div style="font-size:18px;">请在<strong style="color:blue" id="time">300</strong>秒内完成支付</div><br /><img src="" id="qr-img" class="center-block"><br /><img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAYAAAACACAYAAAACsL4LAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQyIDc5LjE2MDkyNCwgMjAxNy8wNy8xMy0wMTowNjozOSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTggKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjk0MTY3OTE2OTlBNjExRTlBQ0E1RDRGQzk2Rjk0MTQwIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjk0MTY3OTE3OTlBNjExRTlBQ0E1RDRGQzk2Rjk0MTQwIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6OTQxNjc5MTQ5OUE2MTFFOUFDQTVENEZDOTZGOTQxNDAiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6OTQxNjc5MTU5OUE2MTFFOUFDQTVENEZDOTZGOTQxNDAiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7FoBq9AAAnm0lEQVR42ux9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" class="center-block zfb">'
                    $('#qr-info').html(html);
                    $('#qr-img').attr("src", res.result.data);
                    $('#money').html(res.result.money.toFixed(2));
                    var time = 300;
                    var timer = window.setInterval(function () {
                        $('#time').html(time);
                        time--;
                        $.ajax({
                            url: "/User/CheckOrder",
                            type: "post",
                            data: { out_trade_no: res.result.order_id },
                            success: function (res) {
                                res = eval('(' + res + ')');
                                if (res.code == 200) {
                                    $('#qr-info').html('<h3>支付成功</h3><br /><h3>请等待美工作图</h3>')
                                    clearInterval(timer);
                                }
                            }
                        });
                        if (time == -1) {
                            $('#qr-info').html('<h3>订单超时</h3><br /><h3>请重新获取图片</h3>')
                            clearInterval(timer);
                        }
                    }, 1000);
                }
                else
                    layer.msg(res.message);
            },
            error: function () {
                layer.msg('访问服务器失败');
            }
        });
    });
});