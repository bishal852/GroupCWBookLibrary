import { Link } from "react-router-dom"

function NotFound() {
  return (
    <div className="not-found">
      <h2>404 - Page Not Found</h2>
      <p>The page you are looking for does not exist.</p>
      <Link to="/login" className="link">
        Go to Login
      </Link>
    </div>
  )
}

export default NotFound
