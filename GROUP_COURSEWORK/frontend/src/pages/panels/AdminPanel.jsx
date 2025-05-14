"use client"

import { useState, useEffect } from "react"
import { adminBookService } from "../../services/adminBookService"
import { bannerService } from "../../services/bannerService"
import { adminService } from "../../services/adminService" // Import the new service

import BookList from "../../components/admin/BookList"
import BookForm from "../../components/admin/BookForm"
import StockUpdateForm from "../../components/admin/StockUpdateForm"
import BannerList from "../../components/admin/BannerList"
import BannerForm from "../../components/admin/BannerForm"

import "../../styles/admin/createSatff.css"

function AdminPanel({ onLogout }) {
  const [activeTab, setActiveTab] = useState("books")
  const [books, setBooks] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")
  const [showBookForm, setShowBookForm] = useState(false)
  const [showStockForm, setShowStockForm] = useState(false)
  const [selectedBook, setSelectedBook] = useState(null)
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [pageSize] = useState(10)

  // Banner state
  const [banners, setBanners] = useState([])
  const [loadingBanners, setLoadingBanners] = useState(true)
  const [bannerError, setBannerError] = useState("")
  const [showBannerForm, setShowBannerForm] = useState(false)
  const [selectedBanner, setSelectedBanner] = useState(null)

  // Staff management state from the existing code
  const [staffData, setStaffData] = useState({
    name: "",
    email: "",
    password: "",
  })
  const [staffError, setStaffError] = useState("")
  const [staffSuccess, setStaffSuccess] = useState("")

  useEffect(() => {
    if (activeTab === "books") {
      fetchBooks()
    } else if (activeTab === "banners") {
      fetchBanners()
    }
  }, [activeTab, currentPage, pageSize])

  const fetchBooks = async () => {
    setLoading(true)
    setError("")
    try {
      const data = await adminBookService.getBooks({
        page: currentPage,
        size: pageSize,
      })
      setBooks(data.items)
      setTotalPages(data.totalPages)
    } catch (error) {
      setError(error.message)
    } finally {
      setLoading(false)
    }
  }

  const fetchBanners = async () => {
    setLoadingBanners(true)
    setBannerError("")
    try {
      const data = await bannerService.getAllBanners()
      setBanners(data)
    } catch (error) {
      setBannerError(error.message)
    } finally {
      setLoadingBanners(false)
    }
  }

  const handleTabChange = (tab) => {
    setActiveTab(tab)
  }

  // Book management handlers
  const handleAddBook = () => {
    setSelectedBook(null)
    setShowBookForm(true)
    setShowStockForm(false)
  }

  const handleEditBook = (book) => {
    setSelectedBook(book)
    setShowBookForm(true)
    setShowStockForm(false)
  }

  const handleUpdateStock = (book) => {
    setSelectedBook(book)
    setShowBookForm(false)
    setShowStockForm(true)
  }

  const handleBookFormSuccess = (book) => {
    setShowBookForm(false)
    fetchBooks()
  }

  const handleStockFormSuccess = (book) => {
    setShowStockForm(false)
    fetchBooks()
  }

  const handleCancelForm = () => {
    setShowBookForm(false)
    setShowStockForm(false)
    setShowBannerForm(false)
  }

  const handlePreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1)
    }
  }

  const handleNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1)
    }
  }

  // Banner management handlers
  const handleAddBanner = () => {
    setSelectedBanner(null)
    setShowBannerForm(true)
  }

  const handleEditBanner = (banner) => {
    setSelectedBanner(banner)
    setShowBannerForm(true)
  }

  const handleDeleteBanner = async (id) => {
    if (window.confirm("Are you sure you want to delete this banner?")) {
      try {
        await bannerService.deleteBanner(id)
        fetchBanners()
      } catch (error) {
        setBannerError(error.message)
      }
    }
  }

  const handleBannerFormSuccess = () => {
    setShowBannerForm(false)
    fetchBanners()
  }

  // Staff management handlers from the existing code
  const handleStaffChange = (e) => {
    setStaffData({
      ...staffData,
      [e.target.name]: e.target.value,
    })
  }

  const handleCreateStaff = async (e) => {
    e.preventDefault()
    setStaffError("")
    setStaffSuccess("")

    try {
      // Use the adminService instead of direct fetch
      await adminService.createStaff(staffData)

      setStaffSuccess("Staff member created successfully!")
      setStaffData({
        name: "",
        email: "",
        password: "",
      })
    } catch (error) {
      setStaffError(error.message)
    }
  }

  return (
    <div>
      {/* Add the API URL test component at the top */}


      <div className="panel-header">
        <h2 className="panel-title">Admin Panel</h2>
        <button onClick={onLogout} className="btn btn-danger">
          Logout
        </button>
      </div>

      <div className="admin-tabs">
        <button
          className={`tab-button ${activeTab === "books" ? "active" : ""}`}
          onClick={() => handleTabChange("books")}
        >
          Book Management
        </button>
        <button
          className={`tab-button ${activeTab === "banners" ? "active" : ""}`}
          onClick={() => handleTabChange("banners")}
        >
          Banner Announcements
        </button>
        <button
          className={`tab-button ${activeTab === "staff" ? "active" : ""}`}
          onClick={() => handleTabChange("staff")}
        >
          Staff Management
        </button>
      </div>

      <div className="panel-content">
        {activeTab === "books" && (
          <div className="books-management">
            {showBookForm ? (
              <BookForm book={selectedBook} onSuccess={handleBookFormSuccess} onCancel={handleCancelForm} />
            ) : showStockForm ? (
              <StockUpdateForm book={selectedBook} onSuccess={handleStockFormSuccess} onCancel={handleCancelForm} />
            ) : (
              <>
                <div className="books-actions">
                  <button className="btn btn-primary" onClick={handleAddBook}>
                    Add New Book
                  </button>
                </div>

                {loading ? (
                  <div className="loading">Loading books...</div>
                ) : error ? (
                  <div className="alert alert-danger">{error}</div>
                ) : (
                  <>
                    <BookList
                      books={books}
                      onEdit={handleEditBook}
                      onUpdateStock={handleUpdateStock}
                      onDelete={() => fetchBooks()}
                      onRefresh={fetchBooks}
                    />

                    {totalPages > 1 && (
                      <div className="pagination">
                        <button className="btn btn-secondary" onClick={handlePreviousPage} disabled={currentPage === 1}>
                          Previous
                        </button>
                        <span className="page-info">
                          Page {currentPage} of {totalPages}
                        </span>
                        <button
                          className="btn btn-secondary"
                          onClick={handleNextPage}
                          disabled={currentPage === totalPages}
                        >
                          Next
                        </button>
                      </div>
                    )}
                  </>
                )}
              </>
            )}
          </div>
        )}

        {activeTab === "banners" && (
          <div className="banners-management">
            {showBannerForm ? (
              <BannerForm banner={selectedBanner} onSuccess={handleBannerFormSuccess} onCancel={handleCancelForm} />
            ) : (
              <>
                <div className="banners-actions">
                  <button className="btn btn-primary" onClick={handleAddBanner}>
                    Create New Banner
                  </button>
                </div>

                {loadingBanners ? (
                  <div className="loading">Loading banners...</div>
                ) : bannerError ? (
                  <div className="alert alert-danger">{bannerError}</div>
                ) : (
                  <BannerList
                    banners={banners}
                    onEdit={handleEditBanner}
                    onDelete={handleDeleteBanner}
                    onRefresh={fetchBanners}
                  />
                )}
              </>
            )}
          </div>
        )}

        {activeTab === "staff" && (
          <div className="admin-section">
            <h3>Create Staff Member</h3>

            {staffError && <div className="alert alert-danger">{staffError}</div>}
            {staffSuccess && <div className="alert alert-success">{staffSuccess}</div>}

            <form onSubmit={handleCreateStaff} className="form-container">
              <div className="form-group">
                <label htmlFor="name">Name</label>
                <input
                  type="text"
                  id="name"
                  name="name"
                  className="form-control"
                  value={staffData.name}
                  onChange={handleStaffChange}
                  required
                />
              </div>

              <div className="form-group">
                <label htmlFor="email">Email</label>
                <input
                  type="email"
                  id="email"
                  name="email"
                  className="form-control"
                  value={staffData.email}
                  onChange={handleStaffChange}
                  required
                />
              </div>

              <div className="form-group">
                <label htmlFor="password">Password</label>
                <input
                  type="password"
                  id="password"
                  name="password"
                  className="form-control"
                  value={staffData.password}
                  onChange={handleStaffChange}
                  required
                />
              </div>

              <button type="submit" className="btn btn-primary">
                Create Staff
              </button>
            </form>
          </div>
        )}
      </div>
    </div>
  )
}

export default AdminPanel
