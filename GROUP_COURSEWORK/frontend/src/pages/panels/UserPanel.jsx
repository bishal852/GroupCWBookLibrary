import BookCatalogue from "../books/BookCatalogue"
import UserNavbar from "../../components/UserNavbar"

function UserPanel({ onLogout }) {
  return (
    <div>
      <UserNavbar onLogout={onLogout} />
      <div className="panel-content">
        <BookCatalogue />
      </div>
    </div>
  )
}

export default UserPanel
