"use client"

import { useState, useEffect } from "react"
import Toast from "./Toast"
import "../styles/ToastContainer.css"

const ToastContainer = () => {
  const [toasts, setToasts] = useState([])

  // Function to add a new toast
  const addToast = (message) => {
    const id = Date.now()
    setToasts((prevToasts) => [...prevToasts, { id, message }])
  }

  // Function to remove a toast
  const removeToast = (id) => {
    setToasts((prevToasts) => prevToasts.filter((toast) => toast.id !== id))
  }

  // Expose the addToast function globally
  useEffect(() => {
    window.addToast = addToast
    return () => {
      delete window.addToast
    }
  }, [])

  return (
    <div className="toast-container">
      {toasts.map((toast) => (
        <Toast key={toast.id} message={toast.message} onClose={() => removeToast(toast.id)} />
      ))}
    </div>
  )
}

export default ToastContainer
