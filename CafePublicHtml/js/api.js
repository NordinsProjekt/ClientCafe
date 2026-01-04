// API Configuration
const API_BASE_URL = 'https://localhost:5001';

// API Service Object
const apiService = {
    /**
     * Fetch all products from the API
     * @returns {Promise<Array>} Array of product objects
     */
    async getAllProducts() {
        try {
            const response = await fetch(`${API_BASE_URL}/products`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const products = await response.json();
            return products;
        } catch (error) {
            console.error('Error fetching products:', error);
            throw error;
        }
    },

    /**
     * Fetch a single product by ID
     * @param {number} id - Product ID
     * @returns {Promise<Object>} Product object
     */
    async getProductById(id) {
        try {
            const response = await fetch(`${API_BASE_URL}/products/${id}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const product = await response.json();
            return product;
        } catch (error) {
            console.error(`Error fetching product ${id}:`, error);
            throw error;
        }
    },

    /**
     * Create a new order
     * @param {Object} orderData - Order data with customerName and items
     * @returns {Promise<Object>} Created order response
     */
    async createOrder(orderData) {
        try {
            const response = await fetch(`${API_BASE_URL}/orders`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(orderData)
            });
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error creating order:', error);
            throw error;
        }
    }
};

// Cart utility functions
const cartUtils = {
    /**
     * Get cart from localStorage
     * @returns {Array} Cart items
     */
    getCart() {
        const cart = localStorage.getItem('cart');
        return cart ? JSON.parse(cart) : [];
    },

    /**
     * Save cart to localStorage
     * @param {Array} cart - Cart items
     */
    saveCart(cart) {
        localStorage.setItem('cart', JSON.stringify(cart));
        this.updateCartCount();
    },

    /**
     * Add item to cart
     * @param {Object} product - Product object
     * @param {number} quantity - Quantity to add
     */
    addToCart(product, quantity = 1) {
        const cart = this.getCart();
        const existingItem = cart.find(item => item.id === product.id);

        if (existingItem) {
            existingItem.quantity += quantity;
        } else {
            cart.push({
                id: product.id,
                name: product.name,
                price: product.price,
                quantity: quantity
            });
        }

        this.saveCart(cart);
    },

    /**
     * Remove item from cart
     * @param {number} productId - Product ID
     */
    removeFromCart(productId) {
        let cart = this.getCart();
        cart = cart.filter(item => item.id !== productId);
        this.saveCart(cart);
    },

    /**
     * Update item quantity in cart
     * @param {number} productId - Product ID
     * @param {number} quantity - New quantity
     */
    updateQuantity(productId, quantity) {
        const cart = this.getCart();
        const item = cart.find(item => item.id === productId);
        
        if (item) {
            item.quantity = quantity;
            if (item.quantity <= 0) {
                this.removeFromCart(productId);
            } else {
                this.saveCart(cart);
            }
        }
    },

    /**
     * Clear entire cart
     */
    clearCart() {
        localStorage.removeItem('cart');
        this.updateCartCount();
    },

    /**
     * Get total cart value
     * @returns {number} Total price
     */
    getCartTotal() {
        const cart = this.getCart();
        return cart.reduce((total, item) => total + (item.price * item.quantity), 0);
    },

    /**
     * Get total item count in cart
     * @returns {number} Total items
     */
    getCartItemCount() {
        const cart = this.getCart();
        return cart.reduce((total, item) => total + item.quantity, 0);
    },

    /**
     * Update cart count badge in navigation
     */
    updateCartCount() {
        const cartCountElement = document.getElementById('cart-count');
        if (cartCountElement) {
            const count = this.getCartItemCount();
            cartCountElement.textContent = count;
        }
    }
};

// Initialize cart count on page load
document.addEventListener('DOMContentLoaded', () => {
    cartUtils.updateCartCount();
});
