"use client"

import { useState } from "react"
import { staffService } from "../../services/staffService"

function StaffPanel({ onLogout }) {
  const [claimCode, setClaimCode] = useState("")
  const [order, setOrder] = useState(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState("")
  const [success, setSuccess] = useState("")
  const [verifyMode, setVerifyMode] = useState(true)

  const handleClaimCodeChange = (e) => {
    setClaimCode(e.target.value.toUpperCase())
    setError("")
  }

  const handleVerifyClaimCode = async (e) => {
    e.preventDefault()

    if (!claimCode.trim()) {
      setError("Please enter a claim code")
      return
    }

    setLoading(true)
    setError("")
    setSuccess("")

    try {
      const orderData = await staffService.getOrderByClaimCode(claimCode)
      setOrder(orderData)
      setVerifyMode(false)
    } catch (error) {
      setError(error.message)
      setOrder(null)
    } finally {
      setLoading(false)
    }
  }

  const handleFulfillOrder = async () => {
    if (!window.confirm("Are you sure you want to fulfill this order?")) {
      return
    }

    setLoading(true)
    setError("")
    setSuccess("")

    try {
      const fulfilledOrder = await staffService.fulfillOrder(claimCode)
      setOrder(fulfilledOrder)
      setSuccess("Order has been successfully fulfilled!")
    } catch (error) {
      setError(error.message)
    } finally {
      setLoading(false)
    }
  }

  const handleReset = () => {
    setClaimCode("")
    setOrder(null)
    setError("")
    setSuccess("")
    setVerifyMode(true)
  }

  const formatDate = (dateString) => {
    if (!dateString) return "N/A"
    return new Date(dateString).toLocaleString()
  }

  return (
    <div>
      <div className="panel-header">
        <h2 className="panel-title">Staff Panel</h2>
        <button onClick={onLogout} className="btn btn-danger">
          Logout
        </button>
      </div>

      <div className="panel-content">
        <div className="staff-section">
          <h3 className="section-title">Order Fulfillment</h3>
          <p className="section-description">
            Enter the claim code provided by the member to verify and fulfill their order.
          </p>

          {error && <div className="alert alert-danger">{error}</div>}
          {success && <div className="alert alert-success">{success}</div>}

          {verifyMode ? (
            <form onSubmit={handleVerifyClaimCode} className="claim-code-form">
              <div className="form-group">
                <label htmlFor="claimCode">Claim Code</label>
                <div className="claim-code-input-group">
                  <input
                    type="text"
                    id="claimCode"
                    className="form-control claim-code-input"
                    value={claimCode}
                    onChange={handleClaimCodeChange}
                    placeholder="Enter claim code (e.g., ABC123)"
                    maxLength="8"
                    disabled={loading}
                  />
                  <button type="submit" className="btn btn-primary" disabled={loading}>
                    {loading ? "Verifying..." : "Verify"}
                  </button>
                </div>
              </div>
            </form>
          ) : (
            <div className="order-details-container">
              <div className="order-verification">
                <h4>
                  Order #{order.id} - Claim Code: <span className="claim-code">{order.claimCode}</span>
                </h4>
                <div className="order-status">
                  Status: <span className={`status-badge status-${order.status.toLowerCase()}`}>{order.status}</span>
                </div>
              </div>

              <div className="customer-info">
                <h4>Customer Information</h4>
                <p>
                  <strong>Name:</strong> {order.customerName}
                </p>
                <p>
                  <strong>Email:</strong> {order.customerEmail}
                </p>
                <p>
                  <strong>Phone:</strong> {order.contactPhone}
                </p>
                <p>
                  <strong>Shipping Address:</strong> {order.shippingAddress}
                </p>
                {order.notes && (
                  <div className="order-notes">
                    <p>
                      <strong>Notes:</strong> {order.notes}
                    </p>
                  </div>
                )}
              </div>

              <div className="order-dates">
                <p>
                  <strong>Order Date:</strong> {formatDate(order.orderDate)}
                </p>
                {order.processedDate && (
                  <p>
                    <strong>Processed Date:</strong> {formatDate(order.processedDate)}
                  </p>
                )}
              </div>

              <div className="order-items">
                <h4>Order Items</h4>
                <table className="items-table">
                  <thead>
                    <tr>
                      <th>Title</th>
                      <th>Author</th>
                      <th>Quantity</th>
                      <th>Price</th>
                      <th>Subtotal</th>
                    </tr>
                  </thead>
                  <tbody>
                    {order.items.map((item) => (
                      <tr key={item.id}>
                        <td>{item.title}</td>
                        <td>{item.author}</td>
                        <td>{item.quantity}</td>
                        <td>${item.unitPrice.toFixed(2)}</td>
                        <td>${item.subtotal.toFixed(2)}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
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
                <button className="btn btn-secondary" onClick={handleReset}>
                  Back
                </button>
                {order.status === "Pending" ? (
                  <button className="btn btn-primary" onClick={handleFulfillOrder} disabled={loading}>
                    {loading ? "Processing..." : "Fulfill Order"}
                  </button>
                ) : (
                  <div className="order-already-processed">
                    This order has already been {order.status.toLowerCase()}.
                  </div>
                )}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  )
}

export default StaffPanel
