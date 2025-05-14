"use client"

import { useState, useEffect } from "react"
import { useParams, Link } from "react-router-dom"
import { orderService } from "../../services/orderService"

function OrderDetails() {
  const { id } = useParams()
  const [order, setOrder] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")

  useEffect(() => {
    fetchOrderDetails()
  }, [id])

  const fetchOrderDetails = async () => {
    setLoading(true)
    try {
      const data = await orderService.getOrderById(Number.parseInt(id))
      setOrder(data)
    } catch (error) {
      setError(error.message)
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return <div className="loading">Loading order details...</div>
  }

  if (error) {
    return <div className="alert alert-danger">{error}</div>
  }

  if (!order) {
    return <div className="alert alert-danger">Order not found</div>
  }

  return (
    <div className="order-details-container">
      <div className="order-header">
        <h2>Order Details</h2>
        <div className="order-status-badge">
          <span className={`status-badge status-${order.status.toLowerCase()}`}>{order.status}</span>
        </div>
      </div>

      {order.status === "Processed" && (
        <div className="order-processed-notification">
          <div className="alert alert-success">
            <i className="check-icon">✓</i> Your order has been processed and is ready for pickup!
            {order.processedDate && (
              <div className="processed-date">Processed on: {new Date(order.processedDate).toLocaleString()}</div>
            )}
          </div>
        </div>
      )}

      <div className="order-info-section">
        <div className="order-meta">
          <div className="info-item">
            <span className="info-label">Order Number:</span>
            <span className="info-value">#{order.id}</span>
          </div>
          <div className="info-item">
            <span className="info-label">Date:</span>
            <span className="info-value">{new Date(order.orderDate).toLocaleDateString()}</span>
          </div>
          <div className="info-item">
            <span className="info-label">Claim Code:</span>
            <span className="info-value claim-code">{order.claimCode}</span>
          </div>
          <div className="info-item">
            <span className="info-label">Status:</span>
            <span className={`info-value status-${order.status.toLowerCase()}`}>{order.status}</span>
          </div>
        </div>

        <div className="shipping-info">
          <h3>Shipping Information</h3>
          <p>{order.shippingAddress}</p>
          <p>Phone: {order.contactPhone}</p>
          {order.notes && (
            <div className="order-notes">
              <h4>Notes:</h4>
              <p>{order.notes}</p>
            </div>
          )}
        </div>
      </div>

      <div className="order-items-section">
        <h3>Order Items</h3>
        <div className="items-list">
          {order.items.map((item) => (
            <div key={item.id} className="order-item">
              <div className="item-image">
                <img
                  src={item.coverImageUrl || "/images/book-placeholder.jpg"}
                  alt={`Cover of ${item.title}`}
                  className="item-cover"
                />
              </div>
              <div className="item-details">
                <h4 className="item-title">{item.title}</h4>
                <p className="item-author">by {item.author}</p>
                <div className="item-price-info">
                  <span className="item-price">
                    ${item.unitPrice.toFixed(2)} x {item.quantity}
                  </span>
                  <span className="item-subtotal">${item.subtotal.toFixed(2)}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>

      <div className="order-summary">
        <div className="summary-row">
          <span className="summary-label">Subtotal:</span>
          <span className="summary-value">${order.subtotal.toFixed(2)}</span>
        </div>
        <div className="summary-row">
          <span className="summary-label">Tax (8%):</span>
          <span className="summary-value">${order.tax.toFixed(2)}</span>
        </div>
        <div className="summary-row total">
          <span className="summary-label">Total:</span>
          <span className="summary-value">${order.total.toFixed(2)}</span>
        </div>
      </div>

      <div className="order-actions">
        {order.status === "Pending" && order.canCancel && (
          <button
            className="btn btn-danger"
            onClick={async () => {
              if (window.confirm("Are you sure you want to cancel this order?")) {
                try {
                  await orderService.cancelOrder(order.id)
                  // Refresh order details
                  fetchOrderDetails()
                } catch (error) {
                  setError(error.message)
                }
              }
            }}
          >
            Cancel Order
          </button>
        )}
        <Link to="/orders" className="btn btn-secondary">
          Back to Orders
        </Link>
      </div>
    </div>
  )
}

export default OrderDetails
