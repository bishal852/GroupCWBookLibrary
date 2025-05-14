"use client"

import { useState } from "react"
import { adminBookService } from "../../services/adminBookService"

function StockUpdateForm({ book, onSuccess, onCancel }) {
  const [stockQuantity, setStockQuantity] = useState(book.stockQuantity)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState("")

  const handleChange = (e) => {
    const value = Number.parseInt(e.target.value)
    if (!isNaN(value) && value >= 0) {
      setStockQuantity(value)
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setIsSubmitting(true)
    setError("")

    try {
      const updatedBook = await adminBookService.updateBookStock(book.id, stockQuantity)
      onSuccess(updatedBook)
    } catch (error) {
      setError(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div className="stock-update-form">
      <h3>Update Stock for "{book.title}"</h3>
      {error && <div className="alert alert-danger">{error}</div>}
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="stockQuantity">Stock Quantity</label>
          <input
            type="number"
            id="stockQuantity"
            min="0"
            value={stockQuantity}
            onChange={handleChange}
            className="form-control"
          />
        </div>
        <div className="form-actions">
          <button type="button" className="btn btn-secondary" onClick={onCancel}>
            Cancel
          </button>
          <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
            {isSubmitting ? "Updating..." : "Update Stock"}
          </button>
        </div>
      </form>
    </div>
  )
}

export default StockUpdateForm
