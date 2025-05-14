"use client"

import { useState, useEffect } from "react"
import { useNavigate, Link } from "react-router-dom"
import { useCart } from "../../contexts/CartContext"
import { orderService } from "../../services/orderService"

function Checkout() {
  const { cart, loading: cartLoading, error: cartError, refreshCart } = useCart()
  const navigate = useNavigate()

  const [formData, setFormData] = useState({
    shippingAddress: "",
    contactPhone: "",
    notes: "",
  })

  const [errors, setErrors] = useState({})
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [serverError, setServerError] = useState("")

  // Debug logging
  useEffect(() => {
    if (cart) {
      console.log("Cart data:", cart)
      console.log("Has bulk discount:", cart.hasBulkDiscount)
      console.log("Has loyalty discount:", cart.hasLoyaltyDiscount)
      console.log("Bulk discount amount:", cart.bulkDiscountAmount)
      console.log("Loyalty discount amount:", cart.loyaltyDiscountAmount)
      console.log("Total discount amount:", cart.discountAmount)
    }
  }, [cart])

  const handleChange = (e) => {
    const { name, value } = e.target
    setFormData({
      ...formData,
      [name]: value,
    })
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors({
        ...errors,
        [name]: "",
      })
    }
  }

  const validateForm = () => {
    const newErrors = {}

    if (!formData.shippingAddress.trim()) {
      newErrors.shippingAddress = "Shipping address is required"
    }

    if (!formData.contactPhone.trim()) {
      newErrors.contactPhone = "Contact phone is required"
    } else if (!/^\d{10,15}$/.test(formData.contactPhone.replace(/\D/g, ""))) {
      newErrors.contactPhone = "Please enter a valid phone number"
    }

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSubmit = async (e) => {
    e.preventDefault()

    // Reset errors
    setServerError("")

    // Validate form
    if (!validateForm()) {
      return
    }

    setIsSubmitting(true)

    try {
      const order = await orderService.createOrder(formData)

      // Refresh cart after successful order
      await refreshCart()

      // Navigate to order confirmation page
      navigate(`/order-confirmation/${order.id}`)
    } catch (error) {
      setServerError(error.message)
      setIsSubmitting(false)
    }
  }

  if (cartLoading) {
    return <div className="loading">Loading cart...</div>
  }

  if (cartError) {
    return <div className="alert alert-danger">{cartError}</div>
  }

  if (!cart || cart.items.length === 0) {
    return (
      <div className="checkout-page">
        <h2 className="checkout-title">Checkout</h2>
        <div className="empty-checkout">
          <p>Your cart is empty. You cannot proceed to checkout.</p>
          <Link to="/user-panel" className="btn btn-primary">
            Browse Books
          </Link>
        </div>
      </div>
    )
  }

  return (
    <div className="checkout-page">
      <h2 className="checkout-title">Checkout</h2>

      <div className="checkout-content">
        <div className="checkout-form-container">
          <h3 className="form-section-title">Shipping Information</h3>

          {serverError && <div className="alert alert-danger">{serverError}</div>}

          <form onSubmit={handleSubmit} className="checkout-form">
            <div className="form-group">
              <label htmlFor="shippingAddress">Shipping Address</label>
              <textarea
                id="shippingAddress"
                name="shippingAddress"
                className={`form-control ${errors.shippingAddress ? "is-invalid" : ""}`}
                value={formData.shippingAddress}
                onChange={handleChange}
                rows={3}
              />
              {errors.shippingAddress && <div className="invalid-feedback">{errors.shippingAddress}</div>}
            </div>

            <div className="form-group">
              <label htmlFor="contactPhone">Contact Phone</label>
              <input
                type="tel"
                id="contactPhone"
                name="contactPhone"
                className={`form-control ${errors.contactPhone ? "is-invalid" : ""}`}
                value={formData.contactPhone}
                onChange={handleChange}
              />
              {errors.contactPhone && <div className="invalid-feedback">{errors.contactPhone}</div>}
            </div>

            <div className="form-group">
              <label htmlFor="notes">Order Notes (Optional)</label>
              <textarea
                id="notes"
                name="notes"
                className="form-control"
                value={formData.notes}
                onChange={handleChange}
                rows={2}
                placeholder="Special instructions for delivery, etc."
              />
            </div>

            <div className="checkout-actions">
              <Link to="/cart" className="btn btn-secondary">
                Back to Cart
              </Link>
              <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
                {isSubmitting ? "Processing..." : "Place Order"}
              </button>
            </div>
          </form>
        </div>

        <div className="order-summary">
          <h3 className="summary-title">Order Summary</h3>
          <div className="summary-items">
            {cart.items.map((item) => (
              <div key={item.id} className="summary-item">
                <div className="item-info">
                  <span className="item-quantity">{item.quantity}x</span>
                  <span className="item-title">{item.title}</span>
                </div>
                <span className="item-price">${item.subtotal.toFixed(2)}</span>
              </div>
            ))}
          </div>
          <div className="summary-totals">
            <div className="summary-row">
              <span className="summary-label">Subtotal:</span>
              <span className="summary-value">${cart.subtotal.toFixed(2)}</span>
            </div>

            {/* Display discounts if applicable */}
            {cart.discountAmount > 0 && (
              <>
                <div className="summary-row discount">
                  <span className="summary-label">Discounts:</span>
                  <span className="summary-value discount-value">-${cart.discountAmount.toFixed(2)}</span>
                </div>

                {cart.hasBulkDiscount && (
                  <div className="summary-row discount-detail">
                    <span className="summary-label">• 5% Discount (5-9 books):</span>
                    <span className="summary-value">-${cart.bulkDiscountAmount.toFixed(2)}</span>
                  </div>
                )}

                {cart.hasLoyaltyDiscount && (
                  <div className="summary-row discount-detail">
                    <span className="summary-label">• 10% Discount (10+ books):</span>
                    <span className="summary-value">-${cart.loyaltyDiscountAmount.toFixed(2)}</span>
                  </div>
                )}

                <div className="summary-row">
                  <span className="summary-label">Discounted Subtotal:</span>
                  <span className="summary-value">${cart.discountedSubtotal.toFixed(2)}</span>
                </div>
              </>
            )}

            <div className="summary-row">
              <span className="summary-label">Tax (8%):</span>
              <span className="summary-value">${cart.tax.toFixed(2)}</span>
            </div>
            <div className="summary-row total">
              <span className="summary-label">Total:</span>
              <span className="summary-value">${cart.total.toFixed(2)}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Checkout
