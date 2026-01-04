// Product Detail Page JavaScript - Well-implemented
// Students should fix the HTML structure and CSS styling in product-detail.html and styles.css

document.addEventListener('DOMContentLoaded', function() {
    const params = new URLSearchParams(window.location.search);
    const productId = params.get('id');
    
    if (!productId) {
        window.location.href = 'products.html';
        return;
    }
    
    loadProductDetail(productId);
});

function loadProductDetail(id) {
    const container = document.getElementById('product-detail');
    
    // Show loading state
    container.innerHTML = '<div class="loading">Loading product...</div>';
    
    apiService.getProductById(id)
        .then(product => {
            displayProductDetail(product);
        })
        .catch(error => {
            console.error('Error loading product:', error);
            container.innerHTML = `
                <div class="error-message">
                    <p>Product not found</p>
                    <a href="products.html" class="btn btn-primary">Back to Products</a>
                </div>
            `;
        });
}

function displayProductDetail(product) {
    const container = document.getElementById('product-detail');
    
    let html = `
        <div class="bad-detail-container">
            <div class="bad-detail-image">
                <div class="bad-image-placeholder">No Image</div>
            </div>
            <div class="bad-detail-info">
                <h2 class="bad-detail-title">${product.name}</h2>
                <p class="bad-detail-description">${product.description || 'No description available'}</p>
                <p class="bad-detail-price">${product.price}</p>
                <div class="bad-quantity-selector">
                    <label>Quantity:
                        <input type="number" id="quantity" value="1" min="1">
                    </label>
                </div>
                <button class="bad-add-button" onclick="addProductToCart(${product.id}, '${product.name.replace(/'/g, "\\'")}}', ${product.price})">Add to Cart</button>
                <a href="products.html" class="bad-back-link">Back to Products</a>
            </div>
        </div>
    `;
    
    container.innerHTML = html;
}

function addProductToCart(id, name, price) {
    const quantityInput = document.getElementById('quantity');
    const quantity = parseInt(quantityInput.value);
    
    // Validate quantity
    if (isNaN(quantity) || quantity < 1) {
        alert('Please enter a valid quantity');
        return;
    }
    
    const product = {
        id: id,
        name: name,
        price: price
    };
    
    cartUtils.addToCart(product, quantity);
    
    // Show confirmation
    alert(`Added ${quantity} item(s) to cart!`);
}
