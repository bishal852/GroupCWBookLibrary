"use client"

import "../../styles/admin/BannerList.css"

const BannerList = ({ banners, onEdit, onDelete, onRefresh }) => {
  const formatDate = (dateString) => {
    const date = new Date(dateString)
    return date.toLocaleDateString()
  }

  const getBannerStatus = (banner) => {
    const now = new Date()
    const startDate = new Date(banner.startDate)
    const endDate = new Date(banner.endDate)

    if (now < startDate) {
      return "Scheduled"
    } else if (now > endDate) {
      return "Expired"
    } else {
      return "Active"
    }
  }

  const getStatusClass = (status) => {
    switch (status) {
      case "Active":
        return "banner-status-active"
      case "Scheduled":
        return "banner-status-scheduled"
      case "Expired":
        return "banner-status-expired"
      default:
        return ""
    }
  }

  return (
    <div className="banner-container">
      <div className="banner-header">
        <h3 className="banner-title">Banner Announcements</h3>
        <button className="banner-refresh-btn" onClick={onRefresh}>
          Refresh
        </button>
      </div>

      {banners.length === 0 ? (
        <div className="banner-empty">No banners found. Create a new banner to get started.</div>
      ) : (
        <div className="banner-table-wrapper">
          <table className="banner-table">
            <thead className="banner-table-head">
              <tr>
                <th className="banner-th">Message</th>
                <th className="banner-th">Start Date</th>
                <th className="banner-th">End Date</th>
                <th className="banner-th">Status</th>
                <th className="banner-th">Actions</th>
              </tr>
            </thead>
            <tbody className="banner-table-body">
              {banners.map((banner) => {
                const status = getBannerStatus(banner)
                return (
                  <tr key={banner.id} className="banner-row">
                    <td className="banner-cell banner-message-cell">
                      <div
                        className="banner-preview"
                        style={{
                          backgroundColor: banner.backgroundColor,
                          color: banner.textColor,
                        }}
                      >
                        {banner.message.length > 100 ? `${banner.message.substring(0, 100)}...` : banner.message}
                      </div>
                    </td>
                    <td className="banner-cell">{formatDate(banner.startDate)}</td>
                    <td className="banner-cell">{formatDate(banner.endDate)}</td>
                    <td className="banner-cell">
                      <span className={`banner-status ${getStatusClass(status)}`}>{status}</span>
                    </td>
                    <td className="banner-cell banner-actions-cell">
                      <div className="banner-buttons">
                        <button
                          className="banner-btn banner-btn-edit"
                          onClick={() => onEdit(banner)}
                          title="Edit Banner"
                        >
                          Edit
                        </button>
                        <button
                          className="banner-btn banner-btn-delete"
                          onClick={() => onDelete(banner.id)}
                          title="Delete Banner"
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        </div>
      )}
    </div>
  )
}

export default BannerList
