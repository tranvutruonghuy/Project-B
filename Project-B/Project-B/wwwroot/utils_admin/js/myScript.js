function deleteForm(id, path) {
    if (confirm('Are you sure you want to delete this category?')) {
        var form = document.createElement('form');
        form.method = 'post';
        form.action = path + id;
        document.body.appendChild(form);
        form.submit();
    }
}

//tao preview img khi add product
document.addEventListener('DOMContentLoaded', function () {
    

    var links = document.querySelectorAll(".tab-link");
    links.forEach(function (link) {
        link.addEventListener("click", function () {
            links.forEach(function (link) {
                link.classList.remove("active");
            });
            this.classList.add("active");

            var tabPanes = document.querySelectorAll(".tab-pane");
            tabPanes.forEach(function (pane) {
                pane.classList.remove("active", "show");
            });
            var targetPane = document.querySelector(this.getAttribute("href"));
            console.log(targetPane);
            targetPane.classList.add("active");
        });
    });

    loadProvince();
    $('#provinceSelect').on('change', function () {
        const parentId = this.value;

        loadDistrict(parentId);
    });
    $('#districtSelect').on('change', function () {
        const parentId = this.value;
        loadWard(parentId);
    });
    $('select').niceSelect();
    console.log('DOM đã sẵn sàng!');


});

function loadDistrict(parentId) {
    fetch('/json/quan_huyen.json')
        .then(response => response.json())
        .then(data => {
            if (!Array.isArray(data)) {
                data = Object.values(data);
            }
            document.getElementById('districtSelect').innerHTML = '';
            const option = document.createElement('option');
            option.value = '';
            option.textContent = 'District';
            option.selected = true;
            document.getElementById('districtSelect').appendChild(option);
            for (const district of data) {
                if (district.parent_code == parentId) {
                    const option = document.createElement('option');
                    option.value = district.code;
                    option.textContent = district.name;
                    document.getElementById('districtSelect').appendChild(option);
                }
            }
            $('#districtSelect').niceSelect('update');
        })
}

function loadProvince() {
    fetch('/json/tinh_tp.json')
        .then(response => response.json())
        .then(data => {
            if (!Array.isArray(data)) {
                data = Object.values(data);
            }

            for (const province of data) {
                const option = document.createElement('option');
                option.value = province.code;
                option.textContent = province.name;
                document.getElementById('provinceSelect').appendChild(option);
            }
            $('#provinceSelect').niceSelect('update');
        })

        .catch(error => console.error('Error:', error));
}

function loadWard(parentId) {
    fetch('/json/xa_phuong.json')
        .then(response => response.json())
        .then(data => {
            if (!Array.isArray(data)) {
                data = Object.values(data);
            }
            document.getElementById('wardSelect').innerHTML = '';
            const option = document.createElement('option');
            option.value = '';
            option.textContent = 'Ward';
            option.selected = true;
            document.getElementById('wardSelect').appendChild(option);
            for (const district of data) {
                if (district.parent_code == parentId) {
                    const option = document.createElement('option');
                    option.value = district.code;
                    option.textContent = district.name;
                    document.getElementById('wardSelect').appendChild(option);
                }
            }
            $('#wardSelect').niceSelect('update');
        })
}

//delete category
function deleteCategory(id) {
    $('#deleteCategoryModal').modal('show');
    $('#confirmDeleteCategory').off('click').on('click', function () {
        var form = document.createElement('form');
        form.method = 'post';
        form.action = '/Admin/DeleteCategory/' + id;
        document.body.appendChild(form);
        form.submit();
    });

}

//chuyen tab sang edit category khi bam vao edit
function editCategory(id) {

    var form = document.createElement('form');
    form.method = 'get';
    form.action = '/Admin/EditCategory/' + id;
    document.body.appendChild(form);
    form.submit();

}

//delete product bang id 
function deleteItem(id) {
    $('#deleteProductModal').modal('show');
    $('#confirmDeleteProduct').off('click').on('click', function () {
        var form = document.createElement('form');
        form.method = 'post';
        form.action = '/Admin/Delete/' + id;
        document.body.appendChild(form);
        form.submit();
    });
}

//edit product bang id
function editItem(id) {

    var form = document.createElement('form');
    form.method = 'get';
    form.action = '/Admin/Edit/' + id;
    document.body.appendChild(form);
    form.submit();

}

// validate truoc khi dang ky tk moi
function checkBeforeSubmit() {

    var usernameList = [];
    var newUsername = $('#usernameRegister').val();
    $.ajax({
        url: '../Client/GetAllUsernames',
        method: 'get',
        success: function (data) {

            usernameList = JSON.stringify(data);

            if (usernameList.includes(newUsername)) {
                alert('Username already exists. Please choose a different username.');

            } else {
                $('#successRegisterModal').modal('show');


            }
        },
        error: function (error) {
            console.log(error);
        }

    });
}

$('#successRegisterModal').on('hidden.bs.modal', function () {
    document.getElementById('register-form').submit();
})

function openOrderModal(button) {
    $('#orderModal').modal('show');
    //alert($(button).attr('id'));
}

//tat modal
function closeModal() {
    $('#successRegisterModal').modal('hide');
    $('#deleteProductModal').modal('hide');
    $('#deleteCategoryModal').modal('hide');
}



// xu ly UI addToCart
function addToCart(productId) {
    var quantity = document.getElementById('quantity-' + productId).value;
    var url = '/Home/AddToCart?id=' + productId + '&quantity=' + quantity;
    $.ajax(
        {
            url: url,
            type: 'POST',
            data: { id: productId, quantity: quantity },
            success: function () {
                refreshCart();

            }
        });

}

//refresh lai cart UI moi lan chuyen tab
function refreshCart() {
    $('#cart-partial').load('/User/WishList/GetCartPartial');
    refreshSmallCart();
}
function closeCart() {
    $('#ltn__utilize-cart-menu').hide();
}
function openCart() {
    $('#ltn__utilize-cart-menu').show();
}
function refreshSmallCart() {
    console.log(window.location.origin);
    $('.mini-cart-icon-partital').load('/User/WishList/GetSmallCartPartial');
}


//update cart o trang cart
function updateCart() {
    var cartItems = [];
    $('.cart-plus-minus-box').each(function () {
        var itemId = $(this).data('id');
        var quantity = $(this).val();
        cartItems.push(
            { id: itemId, quantity: quantity }
        );
    });
    $.ajax(
        {
            url: '/Home/UpdateCart',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(cartItems),
            success: function (response) { 
                alert('Cart updated successfully!');
                location.reload();
            },
            error: function (error) {  
                alert('Error updating cart.');
            }
        });
}






function addToCart() {
    var addToCartButton = document.querySelector('#addToCartButton');
    var productId = addToCartButton.getAttribute('data-product-id');
    console.log(productId);
    var quantityInput = document.querySelector('#quantity-' + productId);
    var quantity = quantityInput.value;
    var url = window.location.origin + '/WishList/Create?productId=' + productId + '&quantity=' + quantity;
    $.ajax({
        url: url,
        type: 'POST',
        data: {
            productId: productId,
            quantity: quantity
        },
        success: function (response) {
            
            console.log(response);
            
            
            refreshSmallCart();
            refreshCart();
        }
    });
}

    $(document).ready(function () {
        $.ajax({
            url: '/Product/LoadCategoryMenu',
            type: 'GET',
            success: function (data) {
                $('#category-menu').html(data);
            },

        });
        refreshCart();
    }
)
function deleteItemFromCart(productID, wishlistID) {
    
    Swal.fire({
        title: 'Are you sure?',
        text: "Do you want to delete this product from your cart?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            console.log("User confirmed the action.");

            $.ajax({
                url: '/WishList/delete/' + wishlistID,
                type: 'POST',
                beforeSend: function () {
                    console.log("Deleting item...");
                },
                success: function (response) {
                    console.log("Success:", response);
                    Swal.fire(
                        'Deleted!',
                        'Item deleted successfully.',
                        'success'
                    );
                    refreshCart();
                },
                error: function (xhr, status, error) {
                    console.error("Error:", error);
                    Swal.fire(
                        'Error!',
                        'An error occurred while deleting the item.',
                        'error'
                    );
                }
            });
        } else {
            console.log("User canceled the action.");
        }
    });

}

function submitOrder() {
    var provinceSelect = document.getElementById('provinceSelect');
    var province = provinceSelect.options[provinceSelect.selectedIndex].textContent;
    console.log(province);
    var wardSelect = document.getElementById('wardSelect');
    var ward = wardSelect.options[wardSelect.selectedIndex].textContent;
    var districtSelect = document.getElementById('districtSelect');
    var district = districtSelect.options[districtSelect.selectedIndex].textContent;
    var street = document.getElementById('checkout-street').value;
    var note = document.getElementById('checkout-note').value;
    var paymentMethod = document.querySelector('input[name="PaymentMethod"]:checked').value;

    var formData = {
        Ward: ward,
        District: district,
        Province: province,
        Street: street,
        Note: note,
        PaymentMethod: paymentMethod
    };

    $.ajax({
        url: '/Checkout/ProceedCheckout',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        statusCode: {
            201: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Failed to Place Order',
                    text: 'Unfortunately, your order could not be processed. Please try again later.'
                });
            }
        },
        success: function (res) {
            if (res.success) {
           
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: res.message 
                });
            } else {
                // Nếu success = false
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi!',
                    text: res.message 
                });
            }
        }
,
        error: function (xhr, status, error) {
            if (xhr.status === 400) {
                // Handle Bad Request error
                $('#myModal .modal-body').text(xhr.responseJSON.message);
                $('#myModal').modal('show');
            } else {
                // Handle other errors
                Swal.fire({
                    icon: 'error',
                    title: 'Oops!',
                    text: 'Something went wrong. Please try again later.'
                });
            }
        }
    });
}

