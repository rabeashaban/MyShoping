function loaddata() {
    console.log("Loading data...");
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetData",
            "dataSrc": "data"
        },
        "columns": [
            { "data": "name" },
            {
                "data": "description",
                "render": function (data) {
                    console.log("Description: ", data); // تحقق من الوصف المُستلم
                    return data;
                }
            },
            { "data": "price" },
            { "data": "categoryName" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Admin/Product/Edit/${data}" class="btn btn-success">Edit</a>
                        <a onClick=DeleteItem("/Admin/Product/Delete/${data}") class="btn btn-danger">Delete</a>
                    `;
                }
            }
        ]
    });
}
