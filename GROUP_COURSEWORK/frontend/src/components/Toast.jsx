"use client"

import { useState, useEffect } from "react"
import "../styles/Toast.css"

const Toast = ({ message, duration = 5000, onClose }) => {
  const [visible, setVisible] = useState(true)

  useEffect(() => {
    const timer = setTimeout(() => {
      setVisible(false)
      if (onClose) {
        setTimeout(onClose, 300) // Allow time for fade-out animation
      }
    }, duration)

    return () => clearTimeout(timer)
  }, [duration, onClose])

  return (
    <div className={`toast ${visible ? "visible" : "hidden"}`}>
      <div className="toast-content">
        <div className="toast-message">{message}</div>
        <button className="toast-close" onClick={() => setVisible(false)}>
          ×
        </button>
      </div>
    </div>
  )
}

export default Toast
