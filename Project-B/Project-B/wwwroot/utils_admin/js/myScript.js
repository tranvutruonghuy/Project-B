function deleteForm(id, path) {
    if (confirm('Are you sure you want to delete this category?')) {
        var form = document.createElement('form');
        form.method = 'post';
        form.action = path + id;
        document.body.appendChild(form);
        form.submit();
    }
}