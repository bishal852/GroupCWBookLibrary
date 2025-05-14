"use client"

import { useState, useEffect } from "react"
import { Link } from "react-router-dom"
import { orderService } from "../../services/orderService"
import UserNavbar from "../../components/UserNavbar"
import "../../styles/OrdersList.css"

function OrdersList() {
  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")

  useEffect(() => {
    fetchOrders()
  }, [])

  const fetchOrders = async () => {
    setLoading(true)
    try {
      const data = await orderService.getOrders()
      setOrders(data)
    } catch (error) {
      setError(error.message)
    } finally {
      setLoading(false)
    }
  }

  if (loading) {
    return (
      <>
        <UserNavbar />
        <div className="loading">Loading orders...</div>
      </>
    )
  }

  if (error) {
    return (
      <>
        <UserNavbar />
        <div className="alert alert-danger">{error}</div>
      </>
    )
  }

  if (orders.length === 0) {
    return (
      <>
        <UserNavbar />
        <div className="users-no-orders">
          <h2>My Orders</h2>
          <div className="alert alert-info">You haven't placed any orders yet.</div>
          <Link to="/books" className="btn btn-primary">
            Browse Books
          </Link>
        </div>
      </>
    )
  }

  return (
    <>
      <UserNavbar />
      <div className="users-orders-list-container">
        <h2>My Orders</h2>
        <div className="users-orders-table">
          <div className="users-orders-table-header">
            <div className="users-orders-table-header-item">Order #</div>
            <div className="users-orders-table-header-item">Date</div>
            <div className="users-orders-table-header-item">Items</div>
            <div className="users-orders-table-header-item">Total</div>
            <div className="users-orders-table-header-item">Claim Code</div>
            <div className="users-orders-table-header-item">Status</div>
            <div className="users-orders-table-header-item">Actions</div>
          </div>
          {orders.map((order) => (
            <div key={order.id} className={`users-order-card users-status-${order.status.toLowerCase()}`}>
              <div className="users-order-card-item">{order.id}</div>
              <div className="users-order-card-item">{new Date(order.orderDate).toLocaleDateString()}</div>
              <div className="users-order-card-item">{order.itemCount} items</div>
              <div className="users-order-card-item">${order.total.toFixed(2)}</div>
              <div className="users-order-card-item users-claim-code">{order.claimCode}</div>
              <div className="users-order-card-item">
                <span className={`users-status-badge users-status-${order.status.toLowerCase()}`}>{order.status}</span>
              </div>
              <div className="users-order-card-item">
                <Link to={`/orders/${order.id}`} className="btn btn-primary">
                  View
                </Link>
                {order.status === "Pending" && (
                  <button
                    className="btn btn-danger"
                    onClick={async () => {
                      if (window.confirm("Are you sure you want to cancel this order?")) {
                        try {
                          await orderService.cancelOrder(order.id)
                          fetchOrders()
                        } catch (error) {
                          setError(error.message)
                        }
                      }
                    }}
                  >
                    Cancel
                  </button>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>
    </>
  )
}

export default OrdersList
