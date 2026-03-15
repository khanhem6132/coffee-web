$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var quantity = 1;
        var tQuantity = $('#quantity_value').text();

        if (tQuantity != '') {
            quantity = parseInt(tQuantity);
        }

        $.ajax({
            url: '/shoppingcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.count);
                    alert(rs.msg);
                }
            }
        });
    });
    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var conf = confirm('Bạn chắc có muốn xóa hết sản phẩm trong giỏ hàng?');
        if (conf == true) {
            $.ajax({
                url: '/shoppingcart/DeteleAll',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.count);
                        $('#trow_' + id).remove();
                        location.reload();
                    }
                }
            });
        }

    });
    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var conf = confirm('Bạn chắc có muốn xóa sản phẩm này khỏi giỏ hàng?');
        if (conf == true) {
            $.ajax({
                url: '/shoppingcart/detele',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.count);
                        $('#trow_' + id).remove();
                        location.reload();
                    }
                }
            });
        }
        
    });
    $(document).on("input", ".quantity", function () {
        var id = $(this).attr("id").split("_")[1]; // lấy ProductId từ input id="Quantity_123"
        var quantity = parseInt($(this).val());

        if (isNaN(quantity) || quantity < 1) quantity = 1;

        var input = $(this); // lưu lại input để dễ tìm row

        $.ajax({
            url: '/shoppingcart/UpdateQuantity',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    // Cập nhật thành tiền cho sản phẩm
                    input.closest("tr").find("th.item-total")
                        .text(rs.totalItem.toLocaleString('vi-VN') + " VND");

                    // Cập nhật tổng giỏ hàng
                    $("tr:last th.text-center")
                        .text(rs.totalCart.toLocaleString('vi-VN') + " VND");
                }
            }
        });
    });

});

function ShowCount() {
    $.ajax({
        url: '/shoppingcart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_items').html(rs.count);
        }
    });
}
function DeteleAll() {
    $.ajax({
        url: '/shoppingcart/DeteleAll',
        type: 'POST',
        success: function (rs) {
            if (rs.success) {
                LoadCart();
            }
        }
    });
}
function LoadCart() {
    $.ajax({
        url: '/shoppingcart/Partital_Item_Cart',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs.count);
        }
    });
}