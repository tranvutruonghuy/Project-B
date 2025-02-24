document.addEventListener('DOMContentLoaded', function () {
    FilePond.registerPlugin(
        FilePondPluginFileEncode,
        FilePondPluginFileValidateSize,
        FilePondPluginImageExifOrientation,
        FilePondPluginImagePreview
    );

    const pond = FilePond.create(document.querySelector('.filepond-input-multiple'), {
        allowReorder: true,
        maxFileSize: '3MB',
        maxFiles: 3
    });

    const form = document.getElementById('add-product-form');
    form.addEventListener('submit', function (e) {
        e.preventDefault(); // Ngăn chặn hành vi submit mặc định của form

        // Tạo đối tượng FormData
        const formData = new FormData(this);

        // Lấy các tệp từ FilePond và thêm vào FormData
        pond.getFiles().forEach(fileItem => {
            formData.append('filepond', fileItem.file);
        });

        // Gửi dữ liệu đến server bằng fetch
        fetch(form.action, {
            method: 'POST',
            body: formData
        })
            .then(response => response.json())
            .then(data => {
                console.log('Success:', data);
                window.location.href = '/admin/product';
                // Xử lý phản hồi từ server
            })
            .catch(error => {
                console.error('Error:', error);
                // Xử lý lỗi
            });
    });
})