"use client"

function SortDropdown({ sortBy, sortDescending, onSortChange }) {
  const handleSortChange = (e) => {
    const value = e.target.value
    const [newSortBy, direction] = value.split("-")
    onSortChange(newSortBy, direction === "desc")
  }

  const currentValue = `${sortBy}-${sortDescending ? "desc" : "asc"}`

  return (
    <div className="sort-dropdown">
      <label htmlFor="sort-select">Sort by:</label>
      <select id="sort-select" value={currentValue} onChange={handleSortChange} className="form-select">
        <option value="CreatedAt-desc">Newest First</option>
        <option value="CreatedAt-asc">Oldest First</option>
        <option value="Title-asc">Title (A-Z)</option>
        <option value="Title-desc">Title (Z-A)</option>
        <option value="Price-asc">Price (Low to High)</option>
        <option value="Price-desc">Price (High to Low)</option>
        <option value="Rating-desc">Highest Rated</option>
        <option value="PublicationDate-desc">Recently Published</option>
        <option value="PublicationDate-asc">Oldest Published</option>
      </select>
    </div>
  )
}

export default SortDropdown
