// Products Page JavaScript - Well-implemented
// Students should fix the HTML structure and CSS styling in products.html and styles.css

document.addEventListener('DOMContentLoaded', function() {
    loadProducts();
});

function loadProducts() {
    const productsList = document.getElementById('products-list');
    const errorMessage = document.getElementById('error-message');
    
    // Show loading state
    productsList.innerHTML = '<div class="loading">Loading products...</div>';
    
    apiService.getAllProducts()
        .then(products => {
            displayProducts(products);
        })
        .catch(error => {
            productsList.innerHTML = '';
            errorMessage.innerHTML = `
                <div class="error-message">
                    <p><strong>Unable to load products.</strong></p>
                    <p>Error: ${error.message}</p>
                    <p>Make sure the API is running at https://localhost:5001</p>
                    <p>Check the browser console (F12) for more details.</p>
                </div>
            `;
            console.error('Error loading products:', error);
            console.error('Full error details:', error);
        });
}

function displayProducts(products) {
    const productsList = document.getElementById('products-list');
    
    if (!products || products.length === 0) {
        productsList.innerHTML = '<p class="empty-state">No products available.</p>';
        return;
    }
    
    let html = '';
    
    products.forEach(product => {
        html += `
            <div class="bad-product-card">
                <div class="bad-product-info">
                    <h3 class="bad-product-name">${product.name}</h3>
                    <p class="bad-product-description">${product.description || 'No description available'}</p>
                    <p class="bad-product-price">${product.price}</p>
                    <div class="bad-product-actions">
                        <a href="product-detail.html?id=${product.id}">View Details</a>
                        <button onclick="addToCart(${product.id}, '${product.name.replace(/'/g, "\\'")}}', ${product.price})">Add to Cart</button>
                    </div>
                </div>
            </div>
        `;
    });
    
    productsList.innerHTML = html;
}

function addToCart(id, name, price) {
    const product = {
        id: id,
        name: name,
        price: price
    };
    
    cartUtils.addToCart(product, 1);
    
    // Show feedback to user
    alert('Added to cart!');
}
