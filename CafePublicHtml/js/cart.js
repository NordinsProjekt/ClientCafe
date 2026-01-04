// Shopping Cart Page - Well-implemented version for reference

document.addEventListener('DOMContentLoaded', function() {
    loadCart();
    setupCheckoutForm();
});

function loadCart() {
    const cart = cartUtils.getCart();
    const cartList = document.getElementById('cart-list');
    const emptyCart = document.getElementById('empty-cart');
    
    if (cart.length === 0) {
        cartList.innerHTML = '';
        emptyCart.style.display = 'block';
        updateCartSummary(0, 0, 0);
        return;
    }
    
    emptyCart.style.display = 'none';
    
    let html = '';
    cart.forEach(item => {
        html += `
            <div class="cart-item" data-product-id="${item.id}">
                <div class="cart-item-info">
                    <h3>${item.name}</h3>
                    <p class="product-price">$${item.price.toFixed(2)}</p>
                </div>
                <div class="cart-item-quantity">
                    <button class="quantity-btn" onclick="updateItemQuantity(${item.id}, ${item.quantity - 1})">-</button>
                    <span>${item.quantity}</span>
                    <button class="quantity-btn" onclick="updateItemQuantity(${item.id}, ${item.quantity + 1})">+</button>
                </div>
                <div class="cart-item-total">
                    <p>$${(item.price * item.quantity).toFixed(2)}</p>
                </div>
                <button class="remove-btn" onclick="removeItem(${item.id})">Remove</button>
            </div>
        `;
    });
    
    cartList.innerHTML = html;
    
    // Calculate totals
    const subtotal = cartUtils.getCartTotal();
    const tax = subtotal * 0.10;
    const total = subtotal + tax;
    
    updateCartSummary(subtotal, tax, total);
}

function updateCartSummary(subtotal, tax, total) {
    document.getElementById('subtotal').textContent = `$${subtotal.toFixed(2)}`;
    document.getElementById('tax').textContent = `$${tax.toFixed(2)}`;
    document.getElementById('total').textContent = `$${total.toFixed(2)}`;
}

function updateItemQuantity(productId, newQuantity) {
    if (newQuantity < 1) {
        removeItem(productId);
        return;
    }
    
    cartUtils.updateQuantity(productId, newQuantity);
    loadCart();
}

function removeItem(productId) {
    if (confirm('Remove this item from cart?')) {
        cartUtils.removeFromCart(productId);
        loadCart();
    }
}

function setupCheckoutForm() {
    const form = document.getElementById('checkout-form');
    const successMessage = document.getElementById('order-success');
    const errorMessage = document.getElementById('order-error');
    
    form.addEventListener('submit', async function(e) {
        e.preventDefault();
        
        const cart = cartUtils.getCart();
        if (cart.length === 0) {
            alert('Your cart is empty!');
            return;
        }
        
        const customerName = document.getElementById('customer-name').value;
        
        // Prepare order data according to API schema
        const orderData = {
            customerName: customerName,
            items: cart.map(item => ({
                productId: item.id,
                quantity: item.quantity
            }))
        };
        
        try {
            // Hide previous messages
            successMessage.style.display = 'none';
            errorMessage.style.display = 'none';
            
            // Disable submit button
            const submitBtn = form.querySelector('button[type="submit"]');
            submitBtn.disabled = true;
            submitBtn.textContent = 'Processing...';
            
            // Create order via API
            const result = await apiService.createOrder(orderData);
            
            console.log('Order created:', result);
            
            // Clear cart
            cartUtils.clearCart();
            
            // Show success message
            successMessage.style.display = 'block';
            form.style.display = 'none';
            
            // Reload cart display
            loadCart();
            
            // Redirect to home after 3 seconds
            setTimeout(() => {
                window.location.href = 'index.html';
            }, 3000);
            
        } catch (error) {
            console.error('Order failed:', error);
            errorMessage.style.display = 'block';
            
            // Re-enable submit button
            const submitBtn = form.querySelector('button[type="submit"]');
            submitBtn.disabled = false;
            submitBtn.textContent = 'Place Order';
        }
    });
}
