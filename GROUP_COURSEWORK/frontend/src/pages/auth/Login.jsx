"use client"

import { useState, useEffect } from "react"
import { Link, useNavigate, useLocation } from "react-router-dom"
import { API_URL } from "../../config"
import "../../styles/login.css"

function Login({ setIsAuthenticated, setUserRole }) {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    role: "Member", // Default role
  })
  const [errors, setErrors] = useState({})
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [serverError, setServerError] = useState("")
  const navigate = useNavigate()
  const location = useLocation()

  // Check if there's a message from redirect
  useEffect(() => {
    if (location.state && location.state.message) {
      setServerError(location.state.message)
    }
  }, [location])

  // Function to set admin credentials for quick login
  const setAdminCredentials = () => {
    setFormData({
      email: "admin@admin.com",
      password: "admin123",
      role: "Admin",
    })
  }

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

  // Handle role selection with buttons
  const handleRoleSelect = (role) => {
    setFormData({
      ...formData,
      role: role,
    })
  }

  const validateForm = () => {
    const newErrors = {}

    // Validate email
    if (!formData.email) {
      newErrors.email = "Email is required"
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = "Email is invalid"
    }

    // Validate password
    if (!formData.password) {
      newErrors.password = "Password is required"
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
      // First try regular login
      let response = await fetch(`${API_URL}/api/auth/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      })

      // If regular login fails and using admin credentials, try test-login
      if (
        !response.ok &&
        formData.email === "admin@admin.com" &&
        formData.password === "admin123" &&
        formData.role === "Admin"
      ) {
        console.log("Regular login failed, trying test login...")
        response = await fetch(`${API_URL}/api/auth/test-login`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(formData),
        })
      }

      if (!response.ok) {
        const errorData = await response.json()
        throw new Error(errorData.message || "Login failed")
      }

      const data = await response.json()

      // Store token in localStorage
      localStorage.setItem("token", data.token)

      // Update authentication state
      setIsAuthenticated(true)
      setUserRole(formData.role)

      // Redirect based on role
      switch (formData.role) {
        case "Member":
          navigate("/user-panel")
          break
        case "Staff":
          navigate("/staff-panel")
          break
        case "Admin":
          navigate("/admin-panel")
          break
        default:
          navigate("/user-panel")
      }
    } catch (error) {
      setServerError(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div className="form-container">
      <h2 className="form-title">Login</h2>

      {serverError && <div className="alert alert-danger">{serverError}</div>}

      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="email">Email Address</label>
          <input
            type="email"
            id="email"
            name="email"
            className={`form-control ${errors.email ? "is-invalid" : ""}`}
            value={formData.email}
            onChange={handleChange}
            placeholder="Enter your email"
          />
          {errors.email && <div className="invalid-feedback">{errors.email}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            name="password"
            className={`form-control ${errors.password ? "is-invalid" : ""}`}
            value={formData.password}
            onChange={handleChange}
            placeholder="Enter your password"
          />
          {errors.password && <div className="invalid-feedback">{errors.password}</div>}
        </div>

        <div className="form-group">
          <label>Select Role</label>
          <div className="role-buttons">
            <button
              type="button"
              className={`role-button ${formData.role === "Member" ? "active" : ""}`}
              onClick={() => handleRoleSelect("Member")}
            >
              Member
            </button>
            <button
              type="button"
              className={`role-button ${formData.role === "Staff" ? "active" : ""}`}
              onClick={() => handleRoleSelect("Staff")}
            >
              Staff
            </button>
            <button
              type="button"
              className={`role-button ${formData.role === "Admin" ? "active" : ""}`}
              onClick={() => handleRoleSelect("Admin")}
            >
              Admin
            </button>
          </div>
        </div>

        <button type="submit" className="btn btn-primary btn-block" disabled={isSubmitting}>
          {isSubmitting ? "Logging in..." : "Login"}
        </button>

        <button type="button" className="btn btn-secondary btn-block mt-2" onClick={setAdminCredentials}>
          Use Admin Credentials
        </button>
      </form>

      <p className="text-center mt-3">
        Don't have an account?{" "}
        <Link to="/register" className="link">
          Register
        </Link>
      </p>
    </div>
  )
}

export default Login
