"use client"

import { useState } from "react"
import { Link } from "react-router-dom"
import { useCart } from "../../contexts/CartContext"
import UserNavbar from "../../components/UserNavbar"

function Cart() {
  const { cart, loading, error, updateCartItem, removeFromCart, clearCart } = useCart()
  const [processingItems, setProcessingItems] = useState(new Set())

  const handleQuantityChange = async (cartItemId, newQuantity) => {
    if (newQuantity < 1) return
    setProcessingItems((prev) => new Set(prev).add(cartItemId))
    await updateCartItem(cartItemId, newQuantity)
    setProcessingItems((prev) => {
      const newSet = new Set(prev)
      newSet.delete(cartItemId)
      return newSet
    })
  }

  const handleRemoveItem = async (cartItemId) => {
    setProcessingItems((prev) => new Set(prev).add(cartItemId))
    await removeFromCart(cartItemId)
    setProcessingItems((prev) => {
      const newSet = new Set(prev)
      newSet.delete(cartItemId)
      return newSet
    })
  }

  const handleClearCart = async () => {
    if (window.confirm("Are you sure you want to clear your cart?")) {
      await clearCart()
    }
  }

  return (
    <>
      <UserNavbar />

      {loading ? (
        <div className="loading">Loading cart...</div>
      ) : error ? (
        <div className="alert alert-danger">{error}</div>
      ) : !cart || cart.items.length === 0 ? (
        <div className="cart-page">
          <h2 className="cart-title">Your Cart</h2>
          <div className="empty-cart">
            <p>Your cart is empty.</p>
            <Link to="/user-panel" className="btn btn-primary">
              Browse Books
            </Link>
          </div>
        </div>
      ) : (
        <div className="cart-page">
          <h2 className="cart-title">Your Cart</h2>

          <div className="cart-actions">
            <Link to="/user-panel" className="btn btn-secondary">
              Continue Shopping
            </Link>
            <button className="btn btn-danger" onClick={handleClearCart}>
              Clear Cart
            </button>
          </div>

          <div className="cart-content">
            <div className="cart-items">
              {cart.items.map((item) => (
                <div key={item.id} className="cart-item">
                  <div className="cart-item-image">
                    <img
                      src={item.coverImageUrl || "/images/book-placeholder.jpg"}
                      alt={`Cover of ${item.title}`}
                      className="cart-cover"
                    />
                  </div>
                  <div className="cart-item-details">
                    <h3 className="cart-item-title">
                      <Link to={`/books/${item.bookId}`}>{item.title}</Link>
                    </h3>
                    <p className="cart-item-author">by {item.author}</p>
                    <div className="cart-item-price">
                      {item.isDiscountActive && <span className="original-price">${item.price.toFixed(2)}</span>}
                      <span className={item.isDiscountActive ? "discount-price" : ""}>${item.currentPrice.toFixed(2)}</span>
                    </div>
                  </div>
                  <div className="cart-item-quantity">
                    <button
                      className="quantity-btn"
                      onClick={() => handleQuantityChange(item.id, item.quantity - 1)}
                      disabled={item.quantity <= 1 || processingItems.has(item.id)}
                    >
                      -
                    </button>
                    <span className="quantity-value">{item.quantity}</span>
                    <button
                      className="quantity-btn"
                      onClick={() => handleQuantityChange(item.id, item.quantity + 1)}
                      disabled={item.quantity >= item.stockQuantity || processingItems.has(item.id)}
                    >
                      +
                    </button>
                  </div>
                  <div className="cart-item-subtotal">
                    <span className="subtotal-label">Subtotal:</span>
                    <span className="subtotal-value">${item.subtotal.toFixed(2)}</span>
                  </div>
                  <div className="cart-item-actions">
                    <button
                      className="btn btn-danger btn-sm"
                      onClick={() => handleRemoveItem(item.id)}
                      disabled={processingItems.has(item.id)}
                    >
                      Remove
                    </button>
                  </div>
                </div>
              ))}
            </div>

            <div className="cart-summary">
              <h3 className="summary-title">Order Summary</h3>
              <div className="summary-row">
                <span className="summary-label">Items ({cart.totalItems}):</span>
                <span className="summary-value">${cart.subtotal.toFixed(2)}</span>
              </div>
              <div className="summary-row">
                <span className="summary-label">Tax (8%):</span>
                <span className="summary-value">${cart.tax.toFixed(2)}</span>
              </div>
              <div className="summary-row total">
                <span className="summary-label">Total:</span>
                <span className="summary-value">${cart.total.toFixed(2)}</span>
              </div>
              <Link to="/checkout" className="btn btn-primary btn-block checkout-btn">
                Proceed to Checkout
              </Link>
            </div>
          </div>
        </div>
      )}
    </>
  )
}

export default Cart
