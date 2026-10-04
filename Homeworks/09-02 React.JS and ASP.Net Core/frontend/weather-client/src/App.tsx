import { useEffect, useState } from 'react'

type WeatherForecast = {
  date: string
  temperatureC: number
  temperatureF: number
  summary: string | null
}

const API_URL = 'http://localhost:5203'

function App() {
  const [items, setItems] = useState<WeatherForecast[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    let cancelled = false

    async function loadWeather() {
      setLoading(true)
      setError(null)

      try {
        const response = await fetch(`${API_URL}/weatherforecast`)
        if (!response.ok) {
          throw new Error(`HTTP ${response.status}`)
        }

        const data = (await response.json()) as WeatherForecast[]
        if (!cancelled) {
          setItems(data)
        }
      } catch (err) {
        if (!cancelled) {
          setError(err instanceof Error ? err.message : 'Не удалось загрузить данные')
        }
      } finally {
        if (!cancelled) {
          setLoading(false)
        }
      }
    }

    void loadWeather()

    return () => {
      cancelled = true
    }
  }, [])

  return (
    <main className="page">
      <h1>Прогноз погоды</h1>
      <p className="subtitle">Данные с API: {API_URL}/weatherforecast</p>

      {loading && <p>Загрузка...</p>}
      {error && <p className="error">Ошибка: {error}</p>}

      {!loading && !error && (
        <table className="weather-table">
          <thead>
            <tr>
              <th>Дата</th>
              <th>°C</th>
              <th>°F</th>
              <th>Описание</th>
            </tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.date}>
                <td>{item.date}</td>
                <td>{item.temperatureC}</td>
                <td>{item.temperatureF}</td>
                <td>{item.summary ?? '—'}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </main>
  )
}

export default App
