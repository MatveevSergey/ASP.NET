export function FactCard({ text }: { text: string }) {
  return (
    <div style={{ background: "#c8e6c9", padding: "12px", borderRadius: "8px" }}>
      {text}
    </div>
  );
}