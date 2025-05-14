"use client"

import { useState, useEffect } from "react"
import { bannerService } from "../../services/bannerService"
import "../../styles/admin/BannerForm.css"

const BannerForm = ({ banner, onSuccess, onCancel }) => {
  const [formData, setFormData] = useState({
    message: "",
    startDate: new Date().toISOString().split("T")[0],
    endDate: new Date(new Date().setDate(new Date().getDate() + 7)).toISOString().split("T")[0],
    backgroundColor: "#f8d7da",
    textColor: "#721c24",
  })

  const [loading, setLoading] = useState(false)
  const [error, setError] = useState("")

  useEffect(() => {
    if (banner) {
      setFormData({
        message: banner.message,
        startDate: new Date(banner.startDate).toISOString().split("T")[0],
        endDate: new Date(banner.endDate).toISOString().split("T")[0],
        backgroundColor: banner.backgroundColor,
        textColor: banner.textColor,
      })
    }
  }, [banner])

  const handleChange = (e) => {
    const { name, value } = e.target
    setFormData({
      ...formData,
      [name]: value,
    })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)
    setError("")

    try {
      console.log("Submitting banner data:", formData)

      // Validate dates
      const startDate = new Date(formData.startDate)
      const endDate = new Date(formData.endDate)

      if (endDate <= startDate) {
        throw new Error("End date must be after start date")
      }

      const bannerData = {
        ...formData,
        startDate: startDate,
        endDate: endDate,
      }

      if (banner) {
        await bannerService.updateBanner(banner.id, bannerData)
      } else {
        await bannerService.createBanner(bannerData)
      }

      onSuccess()
    } catch (error) {
      console.error("Banner form error:", error)
      setError(error.message || "An error occurred while saving the banner")
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="banner-container">
      <h3 className="banner-title">{banner ? "Edit Banner" : "Create New Banner"}</h3>

      {error && <div className="banner-alert">{error}</div>}

      <form onSubmit={handleSubmit} className="banner-form">
        <div className="banner-form-group">
          <label htmlFor="message" className="banner-label">
            Banner Message
          </label>
          <textarea
            id="message"
            name="message"
            className="banner-textarea"
            value={formData.message}
            onChange={handleChange}
            required
            rows={3}
            maxLength={500}
            placeholder="Enter your banner message here..."
          />
        </div>

        <div className="banner-date-row">
          <div className="banner-form-group">
            <label htmlFor="startDate" className="banner-label">
              Start Date
            </label>
            <input
              type="date"
              id="startDate"
              name="startDate"
              className="banner-input"
              value={formData.startDate}
              onChange={handleChange}
              required
            />
          </div>

          <div className="banner-form-group">
            <label htmlFor="endDate" className="banner-label">
              End Date
            </label>
            <input
              type="date"
              id="endDate"
              name="endDate"
              className="banner-input"
              value={formData.endDate}
              onChange={handleChange}
              required
            />
          </div>
        </div>

        <div className="banner-colors-row">
          <div className="banner-form-group">
            <label htmlFor="backgroundColor" className="banner-label">
              Background Color
            </label>
            <div className="banner-color-container">
              <input
                type="color"
                id="backgroundColor"
                name="backgroundColor"
                value={formData.backgroundColor}
                onChange={handleChange}
                className="banner-color-picker"
              />
              <input
                type="text"
                value={formData.backgroundColor}
                onChange={handleChange}
                name="backgroundColor"
                className="banner-color-text"
              />
            </div>
          </div>

          <div className="banner-form-group">
            <label htmlFor="textColor" className="banner-label">
              Text Color
            </label>
            <div className="banner-color-container">
              <input
                type="color"
                id="textColor"
                name="textColor"
                value={formData.textColor}
                onChange={handleChange}
                className="banner-color-picker"
              />
              <input
                type="text"
                value={formData.textColor}
                onChange={handleChange}
                name="textColor"
                className="banner-color-text"
              />
            </div>
          </div>
        </div>

        <div className="banner-preview-section">
          <h4 className="banner-preview-title">Preview</h4>
          <div
            className="banner-preview-box"
            style={{
              backgroundColor: formData.backgroundColor,
              color: formData.textColor,
            }}
          >
            {formData.message || "Banner message will appear here"}
          </div>
        </div>

        <div className="banner-actions">
          <button type="button" className="banner-button banner-button-cancel" onClick={onCancel}>
            Cancel
          </button>
          <button type="submit" className="banner-button banner-button-submit" disabled={loading}>
            {loading ? "Saving..." : banner ? "Update Banner" : "Create Banner"}
          </button>
        </div>
      </form>
    </div>
  )
}

export default BannerForm
