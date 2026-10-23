import React from 'react';

interface FeatureTagProps {
  text: string;
}

function FeatureTag(props: FeatureTagProps) {
  return <span>✓ {props.text} </span>;
}

export default FeatureTag;