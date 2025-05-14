"use client"

import { useState, useEffect } from "react"
import { adminBookService } from "../../services/adminBookService"
import "../../styles/admin/BookForm.css"

function BookForm({ book = null, onSuccess, onCancel }) {
  const isEditMode = !!book

  const [formData, setFormData] = useState({
    title: "",
    author: "",
    isbn: "",
    description: "",
    price: "",
    genre: "",
    language: "",
    format: "",
    publisher: "",
    publicationDate: "",
    stockQuantity: "",
    coverImageUrl: "",
    isOnSale: false,
    discountPrice: "",
    discountStartDate: "",
    discountEndDate: "",
  })

  const [errors, setErrors] = useState({})
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [serverError, setServerError] = useState("")

  // Load book data if in edit mode
  useEffect(() => {
    if (isEditMode && book) {
      setFormData({
        title: book.title,
        author: book.author,
        isbn: book.isbn,
        description: book.description,
        price: book.price,
        genre: book.genre,
        language: book.language,
        format: book.format,
        publisher: book.publisher,
        publicationDate: book.publicationDate ? new Date(book.publicationDate).toISOString().split("T")[0] : "",
        stockQuantity: book.stockQuantity,
        coverImageUrl: book.coverImageUrl || "",
        isOnSale: book.isOnSale,
        discountPrice: book.discountPrice || "",
        discountStartDate: book.discountStartDate ? new Date(book.discountStartDate).toISOString().split("T")[0] : "",
        discountEndDate: book.discountEndDate ? new Date(book.discountEndDate).toISOString().split("T")[0] : "",
      })
    }
  }, [book, isEditMode])

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
    const requiredFields = [
      "title",
      "author",
      "isbn",
      "description",
      "price",
      "genre",
      "language",
      "format",
      "publisher",
      "publicationDate",
      "stockQuantity",
    ]

    requiredFields.forEach((field) => {
      if (!formData[field]) {
        newErrors[field] = `${field.charAt(0).toUpperCase() + field.slice(1)} is required`
      }
    })

    // ISBN validation (simple check for now)
    if (formData.isbn && (formData.isbn.length < 10 || formData.isbn.length > 20)) {
      newErrors.isbn = "ISBN must be between 10 and 20 characters"
    }

    // Price validation
    if (formData.price && (isNaN(formData.price) || Number.parseFloat(formData.price) <= 0)) {
      newErrors.price = "Price must be a positive number"
    }

    // Stock quantity validation
    if (formData.stockQuantity && (isNaN(formData.stockQuantity) || Number.parseInt(formData.stockQuantity) < 0)) {
      newErrors.stockQuantity = "Stock quantity must be a non-negative number"
    }

    // Discount price validation
    if (formData.isOnSale) {
      if (!formData.discountPrice) {
        newErrors.discountPrice = "Discount price is required when on sale"
      } else if (isNaN(formData.discountPrice) || Number.parseFloat(formData.discountPrice) <= 0) {
        newErrors.discountPrice = "Discount price must be a positive number"
      } else if (Number.parseFloat(formData.discountPrice) >= Number.parseFloat(formData.price)) {
        newErrors.discountPrice = "Discount price must be less than regular price"
      }

      if (!formData.discountStartDate) {
        newErrors.discountStartDate = "Discount start date is required when on sale"
      }

      if (!formData.discountEndDate) {
        newErrors.discountEndDate = "Discount end date is required when on sale"
      }

      if (
        formData.discountStartDate &&
        formData.discountEndDate &&
        new Date(formData.discountStartDate) >= new Date(formData.discountEndDate)
      ) {
        newErrors.discountEndDate = "Discount end date must be after start date"
      }
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
      const bookData = {
        ...formData,
        price: Number.parseFloat(formData.price),
        stockQuantity: Number.parseInt(formData.stockQuantity),
        discountPrice: formData.isOnSale ? Number.parseFloat(formData.discountPrice) : null,
        discountStartDate: formData.isOnSale ? formData.discountStartDate : null,
        discountEndDate: formData.isOnSale ? formData.discountEndDate : null,
      }

      let result
      if (isEditMode) {
        result = await adminBookService.updateBook(book.id, bookData)
      } else {
        result = await adminBookService.createBook(bookData)
      }

      onSuccess(result)
    } catch (error) {
      setServerError(error.message)
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <form onSubmit={handleSubmit} className="book-form">
      <h2 className="form-title">{isEditMode ? "Edit Book" : "Add New Book"}</h2>

      {serverError && <div className="alert alert-danger">{serverError}</div>}

      <div className="form-grid">
        <div className="form-group">
          <label htmlFor="title">Title</label>
          <input
            type="text"
            id="title"
            name="title"
            className={`form-control ${errors.title ? "is-invalid" : ""}`}
            value={formData.title}
            onChange={handleChange}
            placeholder="Enter book title"
          />
          {errors.title && <div className="invalid-feedback">{errors.title}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="author">Author</label>
          <input
            type="text"
            id="author"
            name="author"
            className={`form-control ${errors.author ? "is-invalid" : ""}`}
            value={formData.author}
            onChange={handleChange}
            placeholder="Enter author name"
          />
          {errors.author && <div className="invalid-feedback">{errors.author}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="isbn">ISBN</label>
          <input
            type="text"
            id="isbn"
            name="isbn"
            className={`form-control ${errors.isbn ? "is-invalid" : ""}`}
            value={formData.isbn}
            onChange={handleChange}
            placeholder="Enter ISBN number"
          />
          {errors.isbn && <div className="invalid-feedback">{errors.isbn}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="price">Price ($)</label>
          <input
            type="number"
            id="price"
            name="price"
            step="0.01"
            min="0.01"
            className={`form-control ${errors.price ? "is-invalid" : ""}`}
            value={formData.price}
            onChange={handleChange}
            placeholder="0.00"
          />
          {errors.price && <div className="invalid-feedback">{errors.price}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="genre">Genre</label>
          <input
            type="text"
            id="genre"
            name="genre"
            className={`form-control ${errors.genre ? "is-invalid" : ""}`}
            value={formData.genre}
            onChange={handleChange}
            placeholder="Fiction, Non-fiction, etc."
          />
          {errors.genre && <div className="invalid-feedback">{errors.genre}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="language">Language</label>
          <input
            type="text"
            id="language"
            name="language"
            className={`form-control ${errors.language ? "is-invalid" : ""}`}
            value={formData.language}
            onChange={handleChange}
            placeholder="English, Spanish, etc."
          />
          {errors.language && <div className="invalid-feedback">{errors.language}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="format">Format</label>
          <input
            type="text"
            id="format"
            name="format"
            className={`form-control ${errors.format ? "is-invalid" : ""}`}
            value={formData.format}
            onChange={handleChange}
            placeholder="Hardcover, Paperback, etc."
          />
          {errors.format && <div className="invalid-feedback">{errors.format}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="publisher">Publisher</label>
          <input
            type="text"
            id="publisher"
            name="publisher"
            className={`form-control ${errors.publisher ? "is-invalid" : ""}`}
            value={formData.publisher}
            onChange={handleChange}
            placeholder="Enter publisher name"
          />
          {errors.publisher && <div className="invalid-feedback">{errors.publisher}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="publicationDate">Publication Date</label>
          <input
            type="date"
            id="publicationDate"
            name="publicationDate"
            className={`form-control ${errors.publicationDate ? "is-invalid" : ""}`}
            value={formData.publicationDate}
            onChange={handleChange}
          />
          {errors.publicationDate && <div className="invalid-feedback">{errors.publicationDate}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="stockQuantity">Stock Quantity</label>
          <input
            type="number"
            id="stockQuantity"
            name="stockQuantity"
            min="0"
            className={`form-control ${errors.stockQuantity ? "is-invalid" : ""}`}
            value={formData.stockQuantity}
            onChange={handleChange}
            placeholder="0"
          />
          {errors.stockQuantity && <div className="invalid-feedback">{errors.stockQuantity}</div>}
        </div>

        <div className="form-group">
          <label htmlFor="coverImageUrl">Cover Image URL</label>
          <input
            type="text"
            id="coverImageUrl"
            name="coverImageUrl"
            className={`form-control ${errors.coverImageUrl ? "is-invalid" : ""}`}
            value={formData.coverImageUrl}
            onChange={handleChange}
            placeholder="https://example.com/book-cover.jpg"
          />
          {errors.coverImageUrl && <div className="invalid-feedback">{errors.coverImageUrl}</div>}
          {formData.coverImageUrl && (
            <div className="cover-preview">
              <img src={formData.coverImageUrl || "/placeholder.svg"} alt="Book cover preview" />
            </div>
          )}
        </div>

        <div className="form-group form-checkbox">
          <input type="checkbox" id="isOnSale" name="isOnSale" checked={formData.isOnSale} onChange={handleChange} />
          <label htmlFor="isOnSale">On Sale</label>
        </div>

        {formData.isOnSale && (
          <div className="discount-section">
            <h3>Discount Information</h3>
            <div className="discount-fields">
              <div className="form-group">
                <label htmlFor="discountPrice">Discount Price ($)</label>
                <input
                  type="number"
                  id="discountPrice"
                  name="discountPrice"
                  step="0.01"
                  min="0.01"
                  className={`form-control ${errors.discountPrice ? "is-invalid" : ""}`}
                  value={formData.discountPrice}
                  onChange={handleChange}
                  placeholder="0.00"
                />
                {errors.discountPrice && <div className="invalid-feedback">{errors.discountPrice}</div>}
              </div>

              <div className="form-group">
                <label htmlFor="discountStartDate">Discount Start Date</label>
                <input
                  type="date"
                  id="discountStartDate"
                  name="discountStartDate"
                  className={`form-control ${errors.discountStartDate ? "is-invalid" : ""}`}
                  value={formData.discountStartDate}
                  onChange={handleChange}
                />
                {errors.discountStartDate && <div className="invalid-feedback">{errors.discountStartDate}</div>}
              </div>

              <div className="form-group">
                <label htmlFor="discountEndDate">Discount End Date</label>
                <input
                  type="date"
                  id="discountEndDate"
                  name="discountEndDate"
                  className={`form-control ${errors.discountEndDate ? "is-invalid" : ""}`}
                  value={formData.discountEndDate}
                  onChange={handleChange}
                />
                {errors.discountEndDate && <div className="invalid-feedback">{errors.discountEndDate}</div>}
              </div>
            </div>
          </div>
        )}
      </div>

      <div className="form-group full-width">
        <label htmlFor="description">Description</label>
        <textarea
          id="description"
          name="description"
          rows="5"
          className={`form-control ${errors.description ? "is-invalid" : ""}`}
          value={formData.description}
          onChange={handleChange}
          placeholder="Enter book description..."
        ></textarea>
        {errors.description && <div className="invalid-feedback">{errors.description}</div>}
      </div>

      <div className="form-actions">
        <button type="button" className="btn btn-secondary" onClick={onCancel}>
          Cancel
        </button>
        <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
          {isSubmitting ? "Saving..." : isEditMode ? "Update Book" : "Add Book"}
        </button>
      </div>
    </form>
  )
}

export default BookForm
