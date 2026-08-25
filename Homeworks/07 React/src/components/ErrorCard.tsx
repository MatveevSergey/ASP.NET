export function ErrorCard({ message }: { message: string }) {
  return (
    <div style={{ background: "#ffcdd2", padding: "12px", borderRadius: "8px" }}>
      {message}
    </div>
  );
}