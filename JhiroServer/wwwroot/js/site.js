window.setImageModalSrc = (imageUrl) => {
    document.getElementById('modalImage').src = imageUrl;
}

window.showImageModal = () => {
    document.getElementById('imageModal').style.display = 'block';
}

window.hideImageModal = () => {
    document.getElementById('imageModal').style.display = 'none';
}

window.auth = {
    getToken: function () {
        return localStorage.getItem("authToken");
    }
};


