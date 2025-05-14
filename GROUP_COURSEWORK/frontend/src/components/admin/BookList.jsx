"use client"

import { useState, useEffect } from "react"
import { adminBookService } from "../../services/adminBookService"
import BookForm from "./BookForm"
import DiscountForm from "./DiscountForm"
import StockUpdateForm from "./StockUpdateForm"
import "../../styles/admin/BookList.css"
import Swal from "sweetalert2";


function BookList({ books: initialBooks, onBookUpdated, onPageChange, currentPage, totalPages }) {
  const [books, setBooks] = useState([])
  const [selectedBook, setSelectedBook] = useState(null)
  const [isEditModalOpen, setIsEditModalOpen] = useState(false)
  const [isStockModalOpen, setIsStockModalOpen] = useState(false)
  const [isDiscountModalOpen, setIsDiscountModalOpen] = useState(false)
  const [isDeleting, setIsDeleting] = useState(false)
  const [deleteError, setDeleteError] = useState("")

  // Filter out deleted books
  useEffect(() => {
    const filteredBooks = initialBooks.filter((book) => !book.title.startsWith("[DELETED]"))
    setBooks(filteredBooks)
  }, [initialBooks])

  const handleEditClick = (book) => {
    setSelectedBook(book)
    setIsEditModalOpen(true)
  }

  const handleStockClick = (book) => {
    setSelectedBook(book)
    setIsStockModalOpen(true)
  }

  const handleDiscountClick = (book) => {
    setSelectedBook(book)
    setIsDiscountModalOpen(true)
  }

  const handleDeleteClick = async (bookId, bookTitle) => {
    if (!window.confirm(`Are you sure you want to delete "${bookTitle}"? This action cannot be undone.`)) {
      return
    }

    setIsDeleting(true)
    setDeleteError("")

    try {
      console.log(`Attempting to delete book ${bookId}`)
      await adminBookService.deleteBook(bookId)
      console.log(`Book ${bookId} deleted successfully`)

      // Remove the book from the local state immediately
      setBooks((prevBooks) => prevBooks.filter((book) => book.id !== bookId))

      // Notify parent component
      onBookUpdated()
    } catch (error) {
      console.error("Delete error:", error.message)

      // If the book is not found, just refresh the list
      if (error.message.includes("not found")) {
        alert(`Book with ID ${bookId} no longer exists. The book list will be refreshed.`)
        onBookUpdated()
        setIsDeleting(false)
        return
      }

      // If normal delete fails, ask to force delete
      if (
        window.confirm(
          `${error.message}\n\nWould you like to force delete this book? This will remove all references to it.`,
        )
      ) {
        try {
          console.log(`Attempting to force delete book ${bookId}`)
          await adminBookService.deleteBook(bookId, true)
          console.log(`Book ${bookId} force deleted successfully`)

          // Remove the book from the local state immediately
          setBooks((prevBooks) => prevBooks.filter((book) => book.id !== bookId))

          // Notify parent component
          onBookUpdated()
        } catch (forceError) {
          console.error("Force delete error:", forceError.message)

          // If the book is not found, just refresh the list
          if (forceError.message.includes("not found")) {
            alert(`Book with ID ${bookId} no longer exists. The book list will be refreshed.`)
            onBookUpdated()
            setIsDeleting(false)
            return
          }

          setDeleteError(forceError.message)

          // If force delete also fails, it's likely because the book is in orders
          // In this case, we'll inform the user that the book has been marked as deleted
          if (forceError.message.includes("violates foreign key constraint")) {
            alert(
              "The book cannot be physically deleted because it is referenced in orders. It has been marked as deleted and made unavailable for purchase.",
            )
            onBookUpdated()
          }
        }
      } else {
        setDeleteError(error.message)
      }
    } finally {
      setIsDeleting(false)
    }
  }

  const handleFormSuccess = () => {
    setIsEditModalOpen(false)
    setIsStockModalOpen(false)
    setIsDiscountModalOpen(false)
    setSelectedBook(null)
    onBookUpdated()
  }

  const handleFormCancel = () => {
    setIsEditModalOpen(false)
    setIsStockModalOpen(false)
    setIsDiscountModalOpen(false)
    setSelectedBook(null)
  }

  const renderPagination = () => {
    const pages = []
    const maxPagesToShow = 5
    let startPage = Math.max(1, currentPage - Math.floor(maxPagesToShow / 2))
    const endPage = Math.min(totalPages, startPage + maxPagesToShow - 1)

    if (endPage - startPage + 1 < maxPagesToShow) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1)
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(
        <button
          key={i}
          className={`pagination-button ${i === currentPage ? "active" : ""}`}
          onClick={() => onPageChange(i)}
        >
          {i}
        </button>,
      )
    }

    return (
      <div className="pagination">
        <button className="pagination-button" onClick={() => onPageChange(1)} disabled={currentPage === 1}>
          &laquo;
        </button>
        <button
          className="pagination-button"
          onClick={() => onPageChange(currentPage - 1)}
          disabled={currentPage === 1}
        >
          &lsaquo;
        </button>
        {pages}
        <button
          className="pagination-button"
          onClick={() => onPageChange(currentPage + 1)}
          disabled={currentPage === totalPages}
        >
          &rsaquo;
        </button>
        <button
          className="pagination-button"
          onClick={() => onPageChange(totalPages)}
          disabled={currentPage === totalPages}
        >
          &raquo;
        </button>
      </div>
    )
  }

  const isDiscountActive = (book) => {
    return (
      book.isOnSale &&
      book.discountPrice &&
      book.discountStartDate &&
      book.discountEndDate &&
      new Date() >= new Date(book.discountStartDate) &&
      new Date() <= new Date(book.discountEndDate)
    )
  }

  return (
    <div className="book-list-container">
      {deleteError && <div className="alert alert-danger">{deleteError}</div>}

      <div className="book-table-wrapper">
        <table className="book-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Title</th>
              <th>Author</th>
              <th>ISBN</th>
              <th>Price</th>
              <th>Stock</th>
              <th>Discount</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {books.map((book) => (
              <tr key={book.id} className={book.stockQuantity === 0 ? "out-of-stock" : ""}>
                <td className="book-id">{book.id}</td>
                <td className="book-title">{book.title}</td>
                <td className="book-author">{book.author}</td>
                <td className="book-isbn">{book.isbn}</td>
                <td className="book-price">
                  {isDiscountActive(book) ? (
                    <>
                      <span className="original-price">${book.price.toFixed(2)}</span>
                      <span className="discount-price">${book.discountPrice.toFixed(2)}</span>
                    </>
                  ) : (
                    <span>${book.price.toFixed(2)}</span>
                  )}
                </td>
                <td className={`book-stock ${book.stockQuantity === 0 ? "stock-warning" : ""}`}>
                  <span className="stock-number">{book.stockQuantity}</span>
                  {book.stockQuantity === 0 && <span className="stock-label">Out of Stock</span>}
                  {book.stockQuantity > 0 && book.stockQuantity < 5 && <span className="stock-label">Low Stock</span>}
                </td>
                <td className="book-discount">
                  {isDiscountActive(book) ? (
                    <div className="discount-info">
                      <span className="discount-badge">Active</span>
                      <div className="discount-dates">
                        {new Date(book.discountStartDate).toLocaleDateString()} -
                        {new Date(book.discountEndDate).toLocaleDateString()}
                      </div>
                    </div>
                  ) : book.discountEndDate && new Date() > new Date(book.discountEndDate) ? (
                    <span className="discount-expired">Expired</span>
                  ) : book.discountStartDate && new Date() < new Date(book.discountStartDate) ? (
                    <span className="discount-scheduled">Scheduled</span>
                  ) : (
                    <span className="no-discount">None</span>
                  )}
                </td>
                <td className="book-actions">
                  <div className="action-buttons">
                    <button className="btn-action btn-edit" onClick={() => handleEditClick(book)} title="Edit Book">
                      Edit
                    </button>
                    <button
                      className="btn-action btn-stock"
                      onClick={() => handleStockClick(book)}
                      title="Update Stock"
                    >
                      Stock
                    </button>
                    <button
                      className="btn-action btn-discount"
                      onClick={() => handleDiscountClick(book)}
                      title="Set Discount"
                    >
                      Discount
                    </button>
                    <button
                      className="btn-action btn-delete"
                      onClick={() => handleDeleteClick(book.id, book.title)}
                      disabled={isDeleting}
                      title="Delete Book"
                    >
                      Delete
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {totalPages > 1 && renderPagination()}

      {isEditModalOpen && selectedBook && (
        <div className="modal-overlay">
          <div className="modal-content">
            <button className="modal-close" onClick={handleFormCancel}>
              &times;
            </button>
            <BookForm book={selectedBook} onSuccess={handleFormSuccess} onCancel={handleFormCancel} />
          </div>
        </div>
      )}

      {isStockModalOpen && selectedBook && (
        <div className="modal-overlay">
          <div className="modal-content modal-sm">
            <button className="modal-close" onClick={handleFormCancel}>
              &times;
            </button>
            <StockUpdateForm book={selectedBook} onSuccess={handleFormSuccess} onCancel={handleFormCancel} />
          </div>
        </div>
      )}

      {isDiscountModalOpen && selectedBook && (
        <div className="modal-overlay">
          <div className="modal-content modal-md">
            <button className="modal-close" onClick={handleFormCancel}>
              &times;
            </button>
            <DiscountForm book={selectedBook} onSuccess={handleFormSuccess} onCancel={handleFormCancel} />
          </div>
        </div>
      )}
    </div>
  )
}

export default BookList
