class ImageManager
{
    constructor(dialogId, productId)
    {
        this.imageIndexs = {};
        this.response = {};
        this.dialog_content = document.getElementById(dialogId);
        this.productId = productId;
    }

    uploadImage()
    {
        var files = $('#postedFiles')[0].files;
        var id = $('#productId').val();
        var formData = new FormData();
        for (var i = 0; i < files.length; i++)
        {
            formData.append("postedFiles", files[i]);
        }
        formData.append('id', id);
        $.ajax({
            url: '/user/AddImage',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                imageManager.getImages(id);
                let notyf = new Notyf({
                    duration: 3000,
                    dismissible: true,
                    position: { x: 'right', y: 'bottom' }
                });
                notyf.success(
                    "Resim Başarıyla Eklendi"
                );
            }
        });
    }

    openImagesDialog(id)
    {
        this.getImages(id);
    }

    getImages()
    {
        var formData = new FormData();
        formData.append("id", this.productId);
        $.ajax({
            url: "/user/GetAdvertImages",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                imageManager.response = response;
                imageManager.setImages();
            },
        });
    }

    setImages()
    {
        this.dialog_content.innerHTML = "";
        for (var i = 0; i < this.response.length; i++) {
            this.imageIndexs[this.response[i].id] = this.response[i].index;
            this.dialog_content.innerHTML += `
                <div class="product-image-cart">
                    <img src="${this.response[i].path}" style="width:300px;height:215px;object-fit:cover"
                    class="product-image-cart-img mb-1">
                    <div class="" style="width: 100%;margin-bottom: .25rem;padding:0">
                        <button onclick="imageManager.deleteImage('${this.response[i].id}')" 
                            class="button-user text-white" style="width: 100%;">
                                Sil
                        </button>
                    </div>
                    <div class="card-body" style="width: 100%;margin-bottom: .25rem;padding:0">
                        <button class="button-user-dark text-white"
                            style="width: 49%;" onclick="imageManager.increaseIndex('${this.response[i].id}')">    <
                        </button>
                        <button class="button-user-dark" onclick="imageManager.minusIndex('${this.response[i].id}')"
                            style="width: 49%;">    >
                        </button>
                    </div>
                </div>`
        }
    }

    deleteImage(imageId)
    {
        var formData = new FormData();
        formData.append("ImageId", imageId);
        formData.append("AdvertId", this.productId);
        $.ajax({
            url: "/user/DeleteImage",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                imageManager.getImages();
                if (response) {
                    let notyf = new Notyf({
                        duration: 3000,
                        dismissible: true,
                        position: { x: 'right', y: 'bottom' }
                    });
                    notyf.success(
                        "Resim Silme İşlemi Başarılı"
                    );
                }
            }
        });
    }

    setAsMainImage(imageId)
    {
        var formData = new FormData();
        formData.append("ImageId", imageId);
        formData.append("AdvertId", this.productId);
        $.ajax({
            url: "/user/SetAsMainImage",
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                imageManager.getImages();
                if (response) {
                    let notyf = new Notyf({
                        duration: 3000,
                        dismissible: true,
                        position: { x: 'right', y: 'bottom' }
                    });
                    notyf.success(
                        "Ana Resim Güncellendi"
                    );
                }
            }
        });
    }

    minusIndex(imageId)
    {
        let index = this.response.find(object => object["id"] == imageId)["index"];
        if (index != this.response.length - 1)
        {
            this.response[index]["index"] += 1;
            this.response[index + 1]["index"] -= 1;
            this.response = this.response.sort((a, b) => b.index - a.index);
            this.response.reverse();
            this.setImages();
        }
    }

    increaseIndex(imageId)
    {
        let index = this.response.find(object => object["id"] == imageId)["index"];
        if (index != 0) {
            this.response[index]["index"] -= 1;
            this.response[index - 1]["index"] += 1;
            this.response = this.response.sort((a, b) => b.index - a.index);
            this.response.reverse();
            this.setImages();
        }
    }

    saveAlignment()
    {
        const data = [];
        const formData = new FormData();
        this.response.forEach(response => {
            data.push({"imageId": response["id"], "index": response["index"]});
        });
        var alignmentJson = JSON.stringify(data);
        formData.append("alignmentJson", alignmentJson);
        formData.append("advertId", this.productId)
        $.ajax({
            type: "POST",
            url: "/user/saveImageAlignment",
            data: formData,
            contentType: false,
            processData: false,
            success: () => {
                let notyf = new Notyf({
                    duration: 3000,
                    dismissible: true,
                    position: { x: 'right', y: 'bottom' }
                });
                notyf.success(
                    "Sıralama Başarıyla Güncellendi"
                );
            }
        });
    }
}

function openDialog() {
    let element = document.querySelector(".dialog");
    console.log(element);
    if (!element.style.display || element.style.display == "none") {
        element.style.display = "flex";
    }
    else {
        element.style.display = "none";
    }

}

function openDialogById(id) {
    let element = document.getElementById(id);
    if (!element.style.display || element.style.display == "none") {
        element.style.display = "flex";
    }
    else {
        element.style.display = "none";
    }

}