"use client"
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom"
import { useState, useEffect } from "react"
import Register from "./pages/auth/Register"
import Login from "./pages/auth/Login"
import UserPanel from "./pages/panels/UserPanel"
import StaffPanel from "./pages/panels/StaffPanel"
import AdminPanel from "./pages/panels/AdminPanel"
import BookDetails from "./pages/books/BookDetails"
import Wishlist from "./pages/wishlist/Wishlist"
import Cart from "./pages/cart/Cart"
import Checkout from "./pages/checkout/Checkout"
import OrdersList from "./pages/orders/OrdersList"
import OrderDetails from "./pages/orders/OrderDetails"
import OrderConfirmation from "./pages/orders/OrderConfirmation"
import NotFound from "./pages/NotFound"
import Banner from "./components/Banner"
import ToastContainer from "./components/ToastContainer"
import signalRService from "./services/signalRService"
import "./App.css"
import { CartProvider } from "./contexts/CartContext"

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [userRole, setUserRole] = useState(null)

  useEffect(() => {
    const token = localStorage.getItem("token")
    if (token) {
      // Parse the JWT token to get the role
      try {
        const payload = JSON.parse(atob(token.split(".")[1]))
        setUserRole(payload.role)
        setIsAuthenticated(true)
      } catch (error) {
        console.error("Invalid token", error)
        localStorage.removeItem("token")
      }
    }
  }, [])

  // Initialize SignalR connection
  useEffect(() => {
    if (isAuthenticated) {
      signalRService.start()
      return () => {
        signalRService.stop()
      }
    }
  }, [isAuthenticated])

  const handleLogout = () => {
    localStorage.removeItem("token")
    setIsAuthenticated(false)
    setUserRole(null)
  }

  return (
    <Router>
      <CartProvider>
        <div className="app-container">
          <Banner />
          <ToastContainer />
          <Routes>
            <Route path="/" element={<Navigate to="/login" />} />
            <Route path="/register" element={<Register />} />
            <Route
              path="/login"
              element={<Login setIsAuthenticated={setIsAuthenticated} setUserRole={setUserRole} />}
            />

            {/* Protected Routes */}
            <Route
              path="/user-panel"
              element={
                isAuthenticated && (userRole === "Member" || userRole === "User") ? (
                  <UserPanel onLogout={handleLogout} />
                ) : (
                  <Navigate to="/login" state={{ message: "Unauthorized access" }} />
                )
              }
            />

            <Route
              path="/staff-panel"
              element={
                isAuthenticated && userRole === "Staff" ? (
                  <StaffPanel onLogout={handleLogout} />
                ) : (
                  <Navigate to="/login" state={{ message: "Unauthorized access" }} />
                )
              }
            />

            <Route
              path="/admin-panel"
              element={
                isAuthenticated && userRole === "Admin" ? (
                  <AdminPanel onLogout={handleLogout} />
                ) : (
                  <Navigate to="/login" state={{ message: "Unauthorized access" }} />
                )
              }
            />

            {/* Book Details Route */}
            <Route
              path="/books/:id"
              element={
                isAuthenticated ? (
                  <BookDetails />
                ) : (
                  <Navigate to="/login" state={{ message: "Please login to view book details" }} />
                )
              }
            />

            {/* Wishlist Route */}
            <Route
              path="/wishlist"
              element={
                isAuthenticated && (userRole === "Member" || userRole === "User") ? (
                  <Wishlist />
                ) : (
                  <Navigate to="/login" state={{ message: "Please login to view your wishlist" }} />
                )
              }
            />

            {/* Cart Route */}
            <Route
              path="/cart"
              element={
                isAuthenticated && (userRole === "Member" || userRole === "User") ? (
                  <Cart />
                ) : (
                  <Navigate to="/login" state={{ message: "Please login to view your cart" }} />
                )
              }
            />

            {/* Checkout Route */}
            <Route
              path="/checkout"
              element={
                isAuthenticated && (userRole === "Member" || userRole === "User") ? (
                  <Checkout />
                ) : (
                  <Navigate to="/login" state={{ message: "Please login to checkout" }} />
                )
              }
            />

            {/* Orders Routes */}
            <Route
              path="/orders"
              element={
                isAuthenticated && (userRole === "Member" || userRole === "User") ? (
                  <OrdersList />
                ) : (
                  <Navigate to="/login" state={{ message: "Please login to view your orders" }} />
                )
              }
            />

            <Route
              path="/orders/:id"
              element={
                isAuthenticated && (userRole === "Member" || userRole === "User") ? (
                  <OrderDetails />
                ) : (
                  <Navigate to="/login" state={{ message: "Please login to view order details" }} />
                )
              }
            />

            <Route
              path="/order-confirmation/:id"
              element={
                isAuthenticated && (userRole === "Member" || userRole === "User") ? (
                  <OrderConfirmation />
                ) : (
                  <Navigate to="/login" state={{ message: "Please login to view order confirmation" }} />
                )
              }
            />

            <Route path="*" element={<NotFound />} />
          </Routes>
        </div>
      </CartProvider>
    </Router>
  )
}

export default App
