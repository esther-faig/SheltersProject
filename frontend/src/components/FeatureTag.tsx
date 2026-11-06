interface FeatureTagProps {
  text: string;
}

function FeatureTag(props: FeatureTagProps) {
  return <span className="tag">✓ {props.text}</span>;
}

export default FeatureTag;