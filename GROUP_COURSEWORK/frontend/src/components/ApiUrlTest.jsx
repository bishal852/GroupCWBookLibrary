"use client"

import { useEffect, useState } from "react"
import { API_URL } from "../config"

export default function ApiUrlTest() {
  const [apiUrl, setApiUrl] = useState("")

  useEffect(() => {
    // Display the current API_URL value
    setApiUrl(API_URL)
  }, [])

  return (
    <div className="api-url-test" style={{ padding: "10px", margin: "10px", border: "1px solid #ccc" }}>
      <h3>API URL Configuration</h3>
      <p>
        Current API URL: <strong>{apiUrl}</strong>
      </p>
      <p>If this is incorrect, please update the API_URL in config.js</p>
    </div>
  )
}
