"use client"

import { useState } from "react"
import { adminBookService } from "../../services/adminBookService"


function DiscountForm({ book, onSuccess, onCancel }) {
  const [formData, setFormData] = useState({
    bookId: book.id,
    discountPercent: "",
    startDate: "",
    endDate: "",
    onSale: true,
  })

  const [errors, setErrors] = useState({})
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [serverError, setServerError] = useState("")

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target
    setFormData({
      ...formData,
      [name]: type === "checkbox" ? checked : value,
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

    // Required fields
    if (!formData.discountPercent) {
      newErrors.discountPercent = "Discount percentage is required"
    } else if (
      isNaN(formData.discountPercent) ||
      Number(formData.discountPercent) <= 0 ||
      Number(formData.discountPercent) >= 100
    ) {
      newErrors.discountPercent = "Discount percentage must be between 1 and 99"
    }

    if (!formData.startDate) {
      newErrors.startDate = "Start date is required"
    }

    if (!formData.endDate) {
      newErrors.endDate = "End date is required"
    }

    if (formData.startDate && formData.endDate && new Date(formData.startDate) >= new Date(formData.endDate)) {
      newErrors.endDate = "End date must be after start date"
    }

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSubmit = async (e) => {
    e.preventDefault()

    // Reset error
    setServerError("")

    // Validate form
    if (!validateForm()) {
      return
    }

    setIsSubmitting(true)

    try {
      // Prepare data for API
      const discountData = {
        bookId: formData.bookId,
        discountPercent: Number(formData.discountPercent),
        startDate: formData.startDate,
        endDate: formData.endDate,
        onSale: formData.onSale,
      }

      const result = await adminBookService.setDiscount(discountData)
      onSuccess(result)
    } catch (error) {
      setServerError(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  // Calculate the discounted price for preview
  const calculateDiscountedPrice = () => {
    if (!formData.discountPercent || isNaN(formData.discountPercent)) {
      return book.price
    }

    const discountPercent = Number(formData.discountPercent)
    if (discountPercent <= 0 || discountPercent >= 100) {
      return book.price
    }

    const discountedPrice = book.price - (book.price * discountPercent) / 100
    return discountedPrice.toFixed(2)
  }

  return (
    <form onSubmit={handleSubmit} className="discount-form">
      <h2 className="form-title">Discount for "{book.title}"</h2>

      {serverError && <div className="alert alert-danger">{serverError}</div>}

      <div className="form-group">
        <label htmlFor="discountPercent">Discount Percentage (%)</label>
        <input
          type="number"
          id="discountPercent"
          name="discountPercent"
          min="1"
          max="99"
          className={`form-control ${errors.discountPercent ? "is-invalid" : ""}`}
          value={formData.discountPercent}
          onChange={handleChange}
        />
        {errors.discountPercent && <div className="invalid-feedback">{errors.discountPercent}</div>}
      </div>

      <div className="date-row">
        <div className="form-group">
          <label htmlFor="startDate">Start Date</label>
          <input
            type="date"
            id="startDate"
            name="startDate"
            className={`form-control ${errors.startDate ? "is-invalid" : ""}`}
            value={formData.startDate}
            onChange={handleChange}
          />
          {errors.startDate && <div className="invalid-feedback">{errors.startDate}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="endDate">End Date</label>
          <input
            type="date"
            id="endDate"
            name="endDate"
            className={`form-control ${errors.endDate ? "is-invalid" : ""}`}
            value={formData.endDate}
            onChange={handleChange}
          />
          {errors.endDate && <div className="invalid-feedback">{errors.endDate}</div>}
        </div>
      </div>

      <div className="form-group form-checkbox">
        <input type="checkbox" id="onSale" name="onSale" checked={formData.onSale} onChange={handleChange} />
        <label htmlFor="onSale">Mark as "On Sale" (displays sale badge)</label>
      </div>

      <div className="discount-preview">
        <h3>Discount Preview</h3>
        <div className="price-comparison">
          <div className="original-price-display">
            <span className="price-label">Original Price:</span>
            <span className="price-value">${book.price.toFixed(2)}</span>
          </div>
          <div className="discounted-price-display">
            <span className="price-label">Discounted Price:</span>
            <span className="price-value discount">${calculateDiscountedPrice()}</span>
          </div>
          {formData.discountPercent && !isNaN(formData.discountPercent) && (
            <div className="savings-display">
              <span className="price-label">Customer Savings:</span>
              <span className="price-value savings">
                ${(book.price - calculateDiscountedPrice()).toFixed(2)} ({formData.discountPercent}%)
              </span>
            </div>
          )}
        </div>
      </div>

      <div className="form-actions">
        <button type="button" className="btn btn-secondary" onClick={onCancel}>
          Cancel
        </button>
        <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
          {isSubmitting ? "Saving..." : "Apply Discount"}
        </button>
      </div>
    </form>
  )
}

export default DiscountForm
