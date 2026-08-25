import { useState } from "react";
import axios from "axios";
import { FactCard } from "./FactCard";
import { ErrorCard } from "./ErrorCard";

export function FactFetcher() {
  const [fact, setFact] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  async function handleClick() {
    setLoading(true);
    setFact(null);
    setError(null);

    try {
      const response = await axios.get<{ fact: string }>("https://catfact.ninja/fact");
      //const response = await axios.get<{ fact: string }>("https://httpbin.org/status/404");      
      const factText = response.data.fact;
      setFact(factText ?? "Нет факта о кошках");
    } catch (err) {
      if (axios.isAxiosError(err) && err.response) {
        setError(`Ошибка ${err.response.status}: ${err.response.statusText}`);
      } else if (err instanceof Error) {
        setError(err.message);
      } else {
        setError("Неизвестная ошибка");
      }
      } finally {
        setLoading(false);
    }
  }

  return (
    <div>
      <button type="button" onClick={handleClick} disabled={loading}>
        {loading ? "Загрузка..." : "Получить факт"}
      </button>      
      {fact && <FactCard text={fact} />}
      {error && <ErrorCard message={error} />}
    </div>
  );
}